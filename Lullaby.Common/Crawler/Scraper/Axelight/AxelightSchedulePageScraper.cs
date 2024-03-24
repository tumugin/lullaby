namespace Lullaby.Common.Crawler.Scraper.Axelight;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp;
using Events;
using Groups;
using RestSharp;

public partial class AxelightSchedulePageScraper : ISchedulePageScraper
{
    private static readonly string SchedulePageUrl = "https://axelight-official.com/schedule/";
    private readonly IBrowsingContext browsingContext;
    private readonly IEventTypeDetector eventTypeDetector;

    private readonly RestClient restClient;

    public AxelightSchedulePageScraper(
        RestClient restClient,
        IBrowsingContext browsingContext,
        IEventTypeDetector eventTypeDetector
    )
    {
        this.restClient = restClient;
        this.browsingContext = browsingContext;
        this.eventTypeDetector = eventTypeDetector;
    }

    public Type TargetGroup => typeof(Axelight);

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        // Get all schedule detail page urls
        var schedulePageDetailUrls = new List<string>();
        // Get from first page
        var firstPageResult = await this.DownloadAndParseSchedulePageAsync(SchedulePageUrl, cancellationToken);
        schedulePageDetailUrls.AddRange(firstPageResult.SchedulePageDetailUrls);
        // Loop and get from next pages
        if (firstPageResult.HasNextPage)
        {
            var shouldContinue = true;
            var nextPageUrl = firstPageResult.NextPageUrl;
            while (shouldContinue)
            {
                var nextPageResult = await this.DownloadAndParseSchedulePageAsync(nextPageUrl, cancellationToken);
                schedulePageDetailUrls.AddRange(nextPageResult.SchedulePageDetailUrls);
                shouldContinue = nextPageResult.HasNextPage;
                if (nextPageResult.HasNextPage)
                {
                    nextPageUrl = nextPageResult.NextPageUrl;
                }
            }
        }

        // Parse all details pages(it's many so not use parallel)
        var events = new List<GroupEvent>();
        foreach (var schedulePageDetailUrl in schedulePageDetailUrls)
        {
            var result = await this.DownloadAndParseScheduleDetailPageAsync(schedulePageDetailUrl, cancellationToken);
            events.Add(result);
        }

        return events;
    }

    [GeneratedRegex("^(\\d+)\\.(\\d+)\\.(\\d+)")]
    private static partial Regex DatePatternRegex();

    [GeneratedRegex("^開場 / (\\d+):(\\d+)")]
    private static partial Regex OpenTimePattenRegex();

    private async Task<ParseSchedulePageResult> DownloadAndParseSchedulePageAsync(
        string pageUrl,
        CancellationToken cancellationToken
    )
    {
        var request = await this.restClient.GetAsync(new RestRequest(pageUrl), cancellationToken);
        using var document =
            await this.browsingContext.OpenAsync(req => req.Content(request.Content), cancellationToken);

        // Search for next page link
        var scheduleListPageLinks = document.QuerySelectorAll(".pagerSec > ul > li > a");
        var nextSchedulePageUrl = scheduleListPageLinks
            .FirstOrDefault(s => s.TextContent == ">")
            ?.GetAttribute("href");

        // Search for schedule detail page links
        var scheduleDetailPageLinks = document
            .QuerySelectorAll(".scheduleList > section > a")
            .Select(v => v.GetAttribute("href"))
            .Where(v => v != null)
            .Select(v => v!)
            .ToArray();

        return new ParseSchedulePageResult
        {
            SchedulePageDetailUrls = scheduleDetailPageLinks,
            HasNextPage = nextSchedulePageUrl != null,
            NextPageUrl = nextSchedulePageUrl
        };
    }

    private async Task<GroupEvent> DownloadAndParseScheduleDetailPageAsync(
        string pageUrl,
        CancellationToken cancellationToken
    )
    {
        var request = await this.restClient.GetAsync(new RestRequest(pageUrl), cancellationToken);
        using var document =
            await this.browsingContext.OpenAsync(req => req.Content(request.Content), cancellationToken);

        // Event place
        var eventPlace = document.QuerySelector(".place")?.TextContent;

        // Event date
        var dateText = document.QuerySelector(".date");
        var dateTextRegexMatches = dateText != null ? DatePatternRegex().Matches(dateText.TextContent) : null;
        var parsedDate = dateTextRegexMatches is { Count: 1 }
            ? new DateTimeOffset(
                int.Parse(dateTextRegexMatches[0].Groups[1].Value, CultureInfo.InvariantCulture),
                int.Parse(dateTextRegexMatches[0].Groups[2].Value, CultureInfo.InvariantCulture),
                int.Parse(dateTextRegexMatches[0].Groups[3].Value, CultureInfo.InvariantCulture),
                0,
                0,
                0,
                TimeSpan.FromHours(9)
            )
            : throw new InvalidDataException("Date must not be null");

        // Event open time
        var openTimeText = document
            .QuerySelectorAll("dl")
            .FirstOrDefault(e => e.QuerySelector("dt")?.TextContent == "TIME")
            ?.QuerySelector("dd")
            ?.TextContent;
        var openTimeTextRegexMatches = openTimeText != null ? OpenTimePattenRegex().Matches(openTimeText) : null;
        DateTimeOffset? detailedOpenTime = openTimeTextRegexMatches is { Count: 1 }
            ? new DateTimeOffset(
                parsedDate.Year,
                parsedDate.Month,
                parsedDate.Day,
                int.Parse(openTimeTextRegexMatches[0].Groups[1].Value, CultureInfo.InvariantCulture),
                int.Parse(openTimeTextRegexMatches[0].Groups[2].Value, CultureInfo.InvariantCulture),
                0,
                TimeSpan.FromHours(9)
            )
            : null;

        // Create event date time
        IEventDateTime eventDateTime = detailedOpenTime switch
        {
            not null => new DetailedEventDateTime
            {
                EventStartDateTime = detailedOpenTime.Value,
                // 閉場時間は分からないので一旦開場時間の4時間後にしておく
                // だいたい入場0.5~1h + ライブ1h~2h + 特典会2hなので4hくらいで十分だと思われる
                EventEndDateTime = detailedOpenTime.Value.AddHours(4)
            },
            _ => new UnDetailedEventDateTime { EventStartDate = parsedDate, EventEndDate = parsedDate.AddDays(1) }
        };

        // Event title(it seems that it has no title sometimes)
        var eventTitle = document.QuerySelector(".title")?.TextContent ?? "無題のイベント";

        // Info text
        var infoText = document.QuerySelector(".info")?.TextContent
                       ?? throw new InvalidDataException("info must not be null");

        return new GroupEvent
        {
            EventDateTime = eventDateTime,
            EventPlace = eventPlace,
            EventDescription = infoText,
            EventName = eventTitle,
            EventType = this.eventTypeDetector.DetectEventTypeByTitle(eventTitle)
        };
    }

    private sealed class ParseSchedulePageResult
    {
        public required IReadOnlyList<string> SchedulePageDetailUrls { get; init; }

        [MemberNotNullWhen(true, nameof(NextPageUrl))]
        public required bool HasNextPage { get; init; }

        public string? NextPageUrl { get; init; }
    }
}
