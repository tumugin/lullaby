namespace Lullaby.Crawler.Scraper.Aoseka;

using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp;
using Common.Groups;
using Events;
using RestSharp;
using Utility;

public partial class AosekaSchedulePageScraper : ISchedulePageScraper
{
    public const string SchedulePageUrl = "https://lit.link/aosekalive";
    private readonly IBrowsingContext browsingContext;

    private readonly RestClient client;
    private readonly IFullDateGuesser fullDateGuesser;
    private readonly IEventTypeDetector eventTypeDetector;
    private readonly TimeProvider timeProvider;

    public AosekaSchedulePageScraper(
        RestClient client,
        IBrowsingContext browsingContext,
        IFullDateGuesser fullDateGuesser,
        IEventTypeDetector eventTypeDetector,
        TimeProvider timeProvider
    )
    {
        this.client = client;
        this.browsingContext = browsingContext;
        this.fullDateGuesser = fullDateGuesser;
        this.eventTypeDetector = eventTypeDetector;
        this.timeProvider = timeProvider;
    }

    public Type TargetGroup => typeof(Aoseka);

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var downloadedDocument = await this.DownloadDocument(cancellationToken);
        var aosekaEvents = await this.ScrapeRawDocument(downloadedDocument, cancellationToken);

        var tokyoTimezone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
        var fullDates = this.fullDateGuesser.GuessFullDateByUncompletedDate(
            aosekaEvents
                .Select(e => e.Date)
                .ToArray(),
            TimeZoneInfo.ConvertTime(this.timeProvider.GetLocalNow(), tokyoTimezone)
        );

        var convertedEvents = aosekaEvents
            .Select((v, i) => new GroupEvent
            {
                EventName = v.Title,
                EventPlace = v.Place,
                EventDateTime = v switch
                {
                    { ParsedOpenTime: { } } => new DetailedEventDateTime
                    {
                        EventStartDateTime = SetHourAndMinutes(fullDates[i], v.ParsedOpenTime),
                        // 閉場時間は分からないので一旦開場時間の4時間後にしておく
                        // だいたい入場0.5~1h + ライブ1h~2h + 特典会2hなので4hくらいで十分だと思われる
                        EventEndDateTime = SetHourAndMinutes(fullDates[i], v.ParsedOpenTime).AddHours(4)
                    },
                    _ => new UnDetailedEventDateTime
                    {
                        EventStartDate = fullDates[i], EventEndDate = fullDates[i].AddDays(1)
                    }
                },
                EventType = this.eventTypeDetector.DetectEventTypeByTitle(v.Title),
                EventDescription = $"{v.DescriptionLinkUrl ?? string.Empty}\n{v.Description ?? string.Empty}"
            })
            .ToArray();
        return convertedEvents;
    }

    private static DateTimeOffset SetHourAndMinutes(DateTimeOffset dateTimeOffset, ParsedTime parsedTime) =>
        new(
            dateTimeOffset.Year,
            dateTimeOffset.Month,
            dateTimeOffset.Day,
            parsedTime.Hour,
            parsedTime.Minutes,
            0,
            dateTimeOffset.Offset
        );

    private async Task<string> DownloadDocument(CancellationToken cancellationToken)
    {
        var request = await this.client.GetAsync(new RestRequest(SchedulePageUrl), cancellationToken);
        return request.Content ?? throw new InvalidDataException("Response must not be null");
    }

    private async Task<IReadOnlyList<AosekaRawEvent>> ScrapeRawDocument(
        string rawHtml,
        CancellationToken cancellationToken
    )
    {
        using var document = await this.browsingContext.OpenAsync(req => req.Content(rawHtml), cancellationToken);
        var eventElements = document.QuerySelectorAll(".creator-detail-links__col");

        return eventElements
            .Select(e =>
            {
                var titleElement = e.QuerySelector("h2") ??
                                   throw new InvalidDataException("Title element must not be null");
                var descriptionElement =
                    e.QuerySelector("p") ?? throw new InvalidDataException("Description element must not be null");
                var linkElement = e.QuerySelector("a");

                var linkUrl = linkElement?.Attributes["href"]?.Value;

                var parsedTitleMatches = DateTitlePatternRegex().Match(titleElement.TextContent);
                var date = parsedTitleMatches.Groups["date"].Value;
                var title = parsedTitleMatches.Groups["title"].Value;

                var openTimeMatches = OpenTimePatternRegex().Match(descriptionElement.TextContent);
                var parsedLocationMatches = LocationPatternRegex().Match(descriptionElement.TextContent);

                var location = parsedLocationMatches switch
                {
                    { Success: true } => parsedLocationMatches.Groups["location"].Value,
                    { Success: false } => null
                };
                var openTime = openTimeMatches switch
                {
                    { Success: true } => openTimeMatches.Groups["openTime"].Value,
                    { Success: false } => null
                };

                return new AosekaRawEvent
                {
                    Date = date.Trim(),
                    Title = title.Trim(),
                    Place = location?.Trim(),
                    Description = descriptionElement.TextContent.Trim(),
                    OpenTime = openTime?.Trim(),
                    DescriptionLinkUrl = linkUrl?.Trim()
                };
            })
            .ToArray();
    }

    [GeneratedRegex("(?<date>\\d{1,2}\\/\\d{1,2})\\((?<day>[月火水木金土日祝]+)\\)(?<title>.+)")]
    private static partial Regex DateTitlePatternRegex();

    [GeneratedRegex(@"OPEN (?<openTime>\d{1,2}:\d{2})")]
    private static partial Regex OpenTimePatternRegex();

    [GeneratedRegex(@"(\p{So}|\p{Cs}\p{Cs}(\p{Cf}\p{Cs}\p{Cs})*)(?<location>[\w\s-]+)")]
    private static partial Regex LocationPatternRegex();

    [GeneratedRegex("(?<hour>\\d{1,2}):(?<minutes>\\d{1,2})")]
    private static partial Regex TimePatternRegex();

    private sealed class ParsedTime
    {
        public required int Hour { get; init; }
        public required int Minutes { get; init; }
    }

    private sealed class AosekaRawEvent
    {
        public required string Date { get; init; }
        public required string Title { get; init; }
        public string? Place { get; init; }
        public string? Description { get; init; }
        public string? OpenTime { get; init; }
        public string? DescriptionLinkUrl { get; init; }
        public ParsedTime? ParsedOpenTime => this.OpenTime != null ? ParseTime(this.OpenTime) : null;

        private static ParsedTime ParseTime(string timeString)
        {
            var match = TimePatternRegex().Match(timeString);
            return new ParsedTime
            {
                Hour = int.Parse(match.Groups["hour"].Value, CultureInfo.InvariantCulture),
                Minutes = int.Parse(match.Groups["minutes"].Value, CultureInfo.InvariantCulture)
            };
        }
    }
}
