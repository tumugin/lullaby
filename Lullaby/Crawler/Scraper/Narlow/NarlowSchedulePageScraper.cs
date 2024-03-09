namespace Lullaby.Crawler.Scraper.Narlow;

using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp;
using Common.Groups;
using Events;
using Flurl;
using Utils;

public partial class NarlowSchedulePageScraper : ISchedulePageScraper
{
    private readonly IEventTypeDetector eventTypeDetector;
    private readonly IBrowsingContext browsingContext;
    private readonly HttpClient httpClient;

    public Type TargetGroup => typeof(Narlow);

    private static readonly string WebsiteUrl = "https://narlow.net";
    private static readonly string SchedulePageUrl = "https://narlow.net/SCHEDULE";
    private readonly TimeZoneInfo jstTimezone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");

    public NarlowSchedulePageScraper(
        IEventTypeDetector eventTypeDetector,
        IBrowsingContext browsingContext,
        HttpClient httpClient
    )
    {
        this.eventTypeDetector = eventTypeDetector;
        this.browsingContext = browsingContext;
        this.httpClient = httpClient;
    }

    private Uri GetMonthSchedulePageUrlByDate(DateTimeOffset date)
    {
        var dateInJst = TimeZoneInfo.ConvertTime(date, this.jstTimezone);

        return new Url(SchedulePageUrl)
            .SetQueryParam(
                "month",
                // The group is on JST timezone so we need to convert the date to JST.
                dateInJst.ToString("yyyy-MM", CultureInfo.InvariantCulture)
            )
            .ToUri();
    }

    private static List<DateTimeOffset> GetMonthlyTargetDates(
        DateTimeOffset startDateTime,
        DateTimeOffset endDateTime
    )
    {
        var targetDates = new List<DateTimeOffset>();
        var currentDate = startDateTime;
        while (currentDate <= endDateTime)
        {
            targetDates.Add(currentDate);
            currentDate = currentDate.AddMonths(1);
        }

        return targetDates;
    }

    private async Task<IReadOnlyList<Uri>> GetIndividualSchedulePageUrls(
        DateTimeOffset startDateTime,
        DateTimeOffset endDateTime,
        CancellationToken cancellationToken
    )
    {
        var targetDates = GetMonthlyTargetDates(startDateTime, endDateTime);
        var rawHtmls = await targetDates
            .ToAsyncEnumerable()
            .SelectAwait(async date =>
            {
                using var request = await this.httpClient.GetAsync(
                    this.GetMonthSchedulePageUrlByDate(date),
                    cancellationToken
                );
                return await request.Content.ReadAsStringAsync(cancellationToken);
            })
            .ToArrayAsync(cancellationToken);

        return await rawHtmls
            .ToAsyncEnumerable()
            .SelectAwait(async rawHtml =>
                await this.browsingContext.OpenAsync(req => req.Content(rawHtml), cancellationToken))
            .SelectMany(x => x.QuerySelectorAll(".contents__wrapper a").ToAsyncEnumerable())
            .Select(x => x.GetAttribute("href"))
            .Where(x => x != null)
            .Distinct()
            .Select(x => new Url(WebsiteUrl).AppendPathSegment(x).ToUri())
            .ToArrayAsync(cancellationToken);
    }

    [GeneratedRegex("^(\\d+)\\.(\\d+)\\.(\\d+)")]
    private static partial Regex DatePatternRegex();

    [GeneratedRegex("OPEN (\\d+):(\\d+)")]
    private static partial Regex OpenTimePattenRegex();

    private async Task<GroupEvent> ParseIndividualSchedulePage(
        Uri individualSchedulePageUrl,
        CancellationToken cancellationToken
    )
    {
        using var request = await this.httpClient.GetAsync(individualSchedulePageUrl, cancellationToken);
        var rawHtml = await request.Content.ReadAsStringAsync(cancellationToken);
        using var document = await this.browsingContext.OpenAsync(req => req.Content(rawHtml), cancellationToken);

        var eventTitle = document.QuerySelector(".article-header__title")?.TextContent ??
                         throw new InvalidDataException("Event title must not be null");
        var eventDateString = document.QuerySelector(".article-header__description__date")?.TextContent ??
                              throw new InvalidDataException("Event date must not be null");
        var eventDescription = document.QuerySelector(".article-text")?.ToHtml(new TextMarkupFormatter()) ??
                               throw new InvalidDataException("Event description must not be null");

        // 一旦日本時間の0時としてパースして作成する
        var dateTextRegexMatches = DatePatternRegex().Matches(eventDateString);
        var parsedDate = dateTextRegexMatches is { Count: 1 }
            ? new DateTimeOffset(
                int.Parse(dateTextRegexMatches[0].Groups[1].Value, CultureInfo.InvariantCulture),
                int.Parse(dateTextRegexMatches[0].Groups[2].Value, CultureInfo.InvariantCulture),
                int.Parse(dateTextRegexMatches[0].Groups[3].Value, CultureInfo.InvariantCulture),
                0,
                0,
                0,
                this.jstTimezone.BaseUtcOffset
            )
            : throw new InvalidDataException("Date must be parsable string");

        var openTimeTextRegexMatches = OpenTimePattenRegex().Matches(eventDescription);
        DateTimeOffset? detailedOpenTime = openTimeTextRegexMatches is { Count: 1 }
            ? new DateTimeOffset(
                parsedDate.Year,
                parsedDate.Month,
                parsedDate.Day,
                int.Parse(openTimeTextRegexMatches[0].Groups[1].Value, CultureInfo.InvariantCulture),
                int.Parse(openTimeTextRegexMatches[0].Groups[2].Value, CultureInfo.InvariantCulture),
                0,
                this.jstTimezone.BaseUtcOffset
            )
            : null;

        return new GroupEvent
        {
            EventName = eventTitle,
            EventPlace = null,
            EventDateTime = detailedOpenTime != null
                ? new DetailedEventDateTime
                {
                    EventStartDateTime = detailedOpenTime.Value,
                    EventEndDateTime = detailedOpenTime.Value.AddHours(4)
                }
                : new UnDetailedEventDateTime { EventStartDate = parsedDate, EventEndDate = parsedDate.AddDays(1) },
            EventDescription = eventDescription.Trim(),
            EventType = this.eventTypeDetector.DetectEventTypeByTitle(eventTitle)
        };
    }

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var baseDateTimeOffset = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, this.jstTimezone);
        var individualSchedulePageUrls = await this.GetIndividualSchedulePageUrls(
            baseDateTimeOffset.AddMonths(-3),
            baseDateTimeOffset.AddMonths(6),
            cancellationToken
        );
        var groupEvents = await individualSchedulePageUrls
            .ToAsyncEnumerable()
            .SelectAwait(async v => await this.ParseIndividualSchedulePage(v, cancellationToken))
            .ToArrayAsync(cancellationToken);
        return groupEvents;
    }
}
