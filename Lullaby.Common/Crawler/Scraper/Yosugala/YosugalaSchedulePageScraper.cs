namespace Lullaby.Common.Crawler.Scraper.Yosugala;

using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp;
using Events;
using Flurl;
using Groups;
using NodaTime;
using NodaTime.Extensions;
using Utils;

public partial class YosugalaSchedulePageScraper(
    HttpClient client,
    IBrowsingContext browsingContext,
    IEventTypeDetector eventTypeDetector,
    TimeProvider timeProvider
) : ISchedulePageScraper
{
    private const string WebsiteUrl = "https://yosugala.fanpla.jp";
    private const string ScheduleMonthPageUrlConstant = "https://yosugala.fanpla.jp/live_information/schedule/list/";
    private const int MonthRange = 6;

    public Type TargetGroup => typeof(Yosugala);
    private readonly TimeZoneInfo jstTimezone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var urls = await this.GetAllSchedulePageUrlsAsync(cancellationToken);
        return await this.GetAllSchedulePageAndScrapeAsync(urls, cancellationToken);
    }

    private async Task<GroupEvent[]> GetAllSchedulePageAndScrapeAsync(IEnumerable<Uri> pageUris,
        CancellationToken cancellationToken)
    {
        var rawHtmls = await pageUris
            .ToAsyncEnumerable()
            .SelectAwait(async uri =>
            {
                using var request = await client.GetAsync(
                    uri,
                    cancellationToken
                );
                return await request.Content.ReadAsStringAsync(cancellationToken);
            })
            .ToArrayAsync(cancellationToken);

        var groupEvents = await rawHtmls.ToAsyncEnumerable()
            .SelectAwait(async rawHtml =>
                await browsingContext.OpenAsync(req => req.Content(rawHtml), cancellationToken)
            )
            .Select(doc =>
            {
                var title = doc.QuerySelector(".block--title")?.TextContent
                            ?? throw new InvalidDataException("Title must not be null");
                var liTags = doc.QuerySelectorAll("li");
                var rawEventDate = liTags
                    .FirstOrDefault(tag => tag.QuerySelector(".item-tit")?.TextContent == "公演日")
                    ?.QuerySelector(".item-detail")
                    ?.TextContent ?? throw new InvalidDataException("Date must be not null string");
                var rawEventPlace = liTags
                    .FirstOrDefault(tag => tag.QuerySelector(".item-tit")?.TextContent == "開催場所・会場")
                    ?.QuerySelector(".item-detail")
                    ?.TextContent;
                var rawEventTime = liTags
                    .FirstOrDefault(tag => tag.QuerySelector(".item-tit")?.TextContent == "時間")
                    ?.QuerySelector(".item-detail")
                    ?.TextContent;
                var rawEventDetails = liTags
                    .FirstOrDefault(tag => tag.QuerySelector(".item-tit")?.TextContent == "INFO")
                    ?.QuerySelector(".item-detail");

                var regexParsedEventDate = ParseDate(rawEventDate);
                var parsedDate = new DateTimeOffset(
                    regexParsedEventDate.year,
                    regexParsedEventDate.month,
                    regexParsedEventDate.day,
                    0,
                    0,
                    0,
                    this.jstTimezone.BaseUtcOffset
                );
                DateTimeOffset? detailedDate = null;

                if (rawEventTime != null)
                {
                    try
                    {
                        var regexParsedEventTime = ParseTimes(rawEventTime);
                        detailedDate = new DateTimeOffset(
                            regexParsedEventDate.year,
                            regexParsedEventDate.month,
                            regexParsedEventDate.day,
                            regexParsedEventTime.openHour,
                            regexParsedEventTime.openMinute,
                            0,
                            this.jstTimezone.BaseUtcOffset
                        );
                    }
                    catch (ArgumentException)
                    {
                        // do nothing
                    }
                }

                return new GroupEvent
                {
                    EventName = title.Trim(),
                    EventPlace = rawEventPlace?.Trim(),
                    EventDateTime = detailedDate != null
                        ? new DetailedEventDateTime
                        {
                            EventStartDateTime = detailedDate.Value,
                            EventEndDateTime = detailedDate.Value.AddHours(4)
                        }
                        : new UnDetailedEventDateTime
                        {
                            EventStartDate = parsedDate, EventEndDate = parsedDate.AddDays(1)
                        },
                    EventDescription = rawEventDetails?.ToHtml(new TextMarkupFormatter()).Trim() ??
                                       throw new InvalidDataException("Event description must not be null"),
                    EventType = eventTypeDetector.DetectEventTypeByTitle(title.Trim())
                };
            })
            .ToArrayAsync(cancellationToken);

        return groupEvents;
    }

    private static (int year, int month, int day) ParseDate(string input)
    {
        var regex = DateRegex();
        var match = regex.Match(input);

        if (!match.Success)
        {
            throw new ArgumentException("Invalid date format");
        }

        var year = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var month = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        var day = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

        return (year, month, day);
    }

    private static (int openHour, int openMinute, int startHour, int startMinute) ParseTimes(string input)
    {
        var regex = TimeRegex();
        var match = regex.Match(input);

        if (!match.Success || match.Groups.Count != 5)
        {
            throw new ArgumentException("Invalid time format");
        }

        return (
            int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture),
            int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture),
            int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture),
            int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture)
        );
    }

    private async Task<Uri[]> GetAllSchedulePageUrlsAsync(CancellationToken cancellationToken)
    {
        // Calculate page urls
        var timeRangeFrom = TimeZoneInfo.ConvertTime(timeProvider.GetUtcNow(), this.jstTimezone)
            .ToOffsetDateTime()
            .With(DateAdjusters.StartOfMonth);
        var targetDates = Enumerable.Range(0, MonthRange)
            .ToArray()
            .Select(i => timeRangeFrom.With(v => v.PlusMonths(i)))
            .ToArray();

        var urls = targetDates
            .Select(v => new Url(ScheduleMonthPageUrlConstant)
                .SetQueryParam("year", v.ToString("yyyy", CultureInfo.InvariantCulture))
                .SetQueryParam("month", v.ToString("MM", CultureInfo.InvariantCulture))
                .ToUri()
            );
        var rawHtmls = await urls.ToAsyncEnumerable()
            .SelectAwait(async uri =>
            {
                using var request = await client.GetAsync(
                    uri,
                    cancellationToken
                );
                return await request.Content.ReadAsStringAsync(cancellationToken);
            })
            .ToArrayAsync(cancellationToken);
        return await rawHtmls
            .ToAsyncEnumerable()
            .SelectAwait(async rawHtml =>
                await browsingContext.OpenAsync(req => req.Content(rawHtml), cancellationToken))
            .SelectMany(x => x.QuerySelectorAll("a").ToAsyncEnumerable())
            .Select(x => x.GetAttribute("href"))
            .Where(x => x != null)
            .Distinct()
            .Where(x => x?.StartsWith("/live_information/detail/", StringComparison.InvariantCulture) ?? false)
            .Select(x => new Url(WebsiteUrl).AppendPathSegment(x).ToUri())
            .ToArrayAsync(cancellationToken);
    }

    [GeneratedRegex(@"^(\d{4})\.(\d{2})\.(\d{2})\[[A-Z]{3}\]$")]
    private static partial Regex DateRegex();

    [GeneratedRegex(@"開場(\d{2}):(\d{2}) - 開演(\d{2}):(\d{2})")]
    private static partial Regex TimeRegex();
}
