namespace Lullaby.Crawler.Scraper.Ryzm;

using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using AngleSharp;
using Events;
using Microsoft.AspNetCore.WebUtilities;
using RestSharp;

public abstract partial class RyzmSchedulePageScraper
{
    private readonly IBrowsingContext browsingContext;

    private readonly RestClient client;
    private readonly IEventTypeDetector eventTypeDetector;

    protected RyzmSchedulePageScraper(
        RestClient client,
        IBrowsingContext browsingContext,
        IEventTypeDetector eventTypeDetector
    )
    {
        this.client = client;
        this.browsingContext = browsingContext;
        this.eventTypeDetector = eventTypeDetector;
    }

    public abstract string SchedulePageUrl { get; }

    private async Task<string> DownloadDocument(int page, CancellationToken cancellationToken)
    {
        var requestUri = new Uri(
            QueryHelpers.AddQueryString(
                this.SchedulePageUrl,
                new Dictionary<string, string?> { { "page", page != 1 ? $"{page}" : null } }
            )
        );
        var request = await this.client.GetAsync(new RestRequest(requestUri), cancellationToken);
        return request.Content ?? throw new InvalidDataException("Response must not be null");
    }

    private async Task<RyzmScheduleObject.RyzmScheduleRootObject> ScrapeRawDocument(
        string rawHtml,
        CancellationToken cancellationToken
    )
    {
        using var document = await this.browsingContext.OpenAsync(req => req.Content(rawHtml), cancellationToken);
        var element = document.QuerySelector("#__NEXT_DATA__") ??
                      throw new InvalidDataException("data element was not found");
        var ryzmSchedulePageObject =
            JsonSerializer.Deserialize<RyzmScheduleObject.RyzmScheduleRootObject>(element.TextContent);
        return ryzmSchedulePageObject!;
    }

    [GeneratedRegex("(\\d+):(\\d+)")]
    private static partial Regex DoorTimeRegex();

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        // 最初のページをスクレイピングして必要なページ数をもらう
        var firstPageObject =
            await this.ScrapeRawDocument(await this.DownloadDocument(1, cancellationToken), cancellationToken);
        var pageCount = firstPageObject.Props.PageProps.Data.FetchedData.LiveList.Meta.LastPage;

        // 2ページ目以降を取得する
        var pageRange = pageCount > 1 ? Enumerable.Range(2, pageCount - 1) : Array.Empty<int>();
        var pageObjects = await Task.WhenAll(
            pageRange.Select(
                async page =>
                    await this.ScrapeRawDocument(await this.DownloadDocument(page, cancellationToken),
                        cancellationToken)
            )
        );
        var allPageSchedules =
            firstPageObject.Props.PageProps.Data.FetchedData.LiveList.Data
                .Concat(
                    pageObjects.SelectMany(page => page.Props.PageProps.Data.FetchedData.LiveList.Data)
                );

        return allPageSchedules
            .Select(rawSchedule =>
            {
                // JSTを基準とした日付が入っているのでそれを使用する
                var eventStartDate = DateTimeOffset
                    .Parse($"{rawSchedule.EventDate} 00:00:00+09:00", CultureInfo.InvariantCulture);
                var parsedDoorTime = rawSchedule.DoorsStartsTime != null
                    ? DoorTimeRegex().Matches(rawSchedule.DoorsStartsTime)
                    : null;
                DateTimeOffset? detailedOpenTime = parsedDoorTime is { Count: 2 }
                    ? new DateTimeOffset(
                        eventStartDate.Year,
                        eventStartDate.Month,
                        eventStartDate.Day,
                        int.Parse(parsedDoorTime[0].Groups[1].Value, CultureInfo.InvariantCulture),
                        int.Parse(parsedDoorTime[0].Groups[2].Value, CultureInfo.InvariantCulture),
                        0,
                        eventStartDate.Offset
                    )
                    : null;
                IEventDateTime eventDateTime = detailedOpenTime switch
                {
                    not null => new DetailedEventDateTime
                    {
                        EventStartDateTime = detailedOpenTime.Value,
                        // 閉場時間は分からないので一旦開場時間の4時間後にしておく
                        // だいたい入場0.5~1h + ライブ1h~2h + 特典会2hなので4hくらいで十分だと思われる
                        EventEndDateTime = detailedOpenTime.Value.AddHours(4)
                    },
                    _ => new UnDetailedEventDateTime
                    {
                        EventStartDate = eventStartDate, EventEndDate = eventStartDate.AddDays(1)
                    }
                };

                var ticketUrls = (rawSchedule
                        .ReservationSetting
                        .Platforms ?? Array.Empty<RyzmScheduleObject.Platform>())
                    .Select(v => v.Url.ToString());
                var joinedTicketUrls = string.Join("\n", ticketUrls);

                return new GroupEvent
                {
                    EventName = rawSchedule.Title,
                    EventPlace = rawSchedule.Venue,
                    EventDateTime = eventDateTime,
                    EventType = this.eventTypeDetector.DetectEventTypeByTitle(rawSchedule.Title),
                    EventDescription = $"チケット代: {rawSchedule.Price}\n出演: {rawSchedule.Artist}\n\n{joinedTicketUrls}"
                };
            })
            .ToArray();
    }
}
