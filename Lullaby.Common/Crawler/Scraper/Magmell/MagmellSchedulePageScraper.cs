namespace Lullaby.Common.Crawler.Scraper.Magmell;

using System.Text.RegularExpressions;
using AngleSharp;
using Events;
using Groups;
using Utility;
using Utils;

public partial class MagmellSchedulePageScraper(
    IBrowsingContext browsingContext,
    HttpClient client,
    IFullDateGuesser fullDateGuesser,
    TimeProvider timeProvider,
    IEventTypeDetector eventTypeDetector
) : ISchedulePageScraper
{
    public Type TargetGroup => typeof(Magmell);

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var downloadedDocument = await this.DownloadDocument(cancellationToken);
        var events = await this.ScrapeRawDocument(downloadedDocument, cancellationToken);

        var tokyoTimezone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
        var fullDates = fullDateGuesser.GuessFullDateByUncompletedDate(
            events
                .Select(e => e.Date)
                .ToArray(),
            TimeZoneInfo.ConvertTime(timeProvider.GetLocalNow(), tokyoTimezone)
        );

        var convertedEvents = events
            .Select((v, i) => new GroupEvent
            {
                EventName = v.Title,
                EventPlace = v.Place,
                EventDateTime =
                    new UnDetailedEventDateTime
                    {
                        EventStartDate = fullDates[i], EventEndDate = fullDates[i].AddDays(1)
                    },
                EventType = eventTypeDetector.DetectEventTypeByTitle(v.Title),
                EventDescription = $"{v.Description ?? string.Empty}\n{v.LinkTo ?? string.Empty}"
            })
            .ToArray();
        return convertedEvents;
    }

    private const string SchedulePageUrl = "https://lit.link/magmellinfo";

    private async Task<string> DownloadDocument(CancellationToken cancellationToken)
    {
        using var request = await client.GetAsync(SchedulePageUrl, cancellationToken);
        return await request.Content.ReadAsStringAsync(cancellationToken);
    }

    [GeneratedRegex("^(?<date>\\d{1,2}/\\d{1,2})\\((?<day>[月火水木金土日祝]+)\\)(?<title>.+)at(?<place>.+)$")]
    private static partial Regex EventNameFullRegex();

    [GeneratedRegex("^(?<date>\\d{1,2}/\\d{1,2})\\((?<day>[月火水木金土日祝]+)\\)(?<title>.+)$")]
    private static partial Regex EventNameRegex();

    private async Task<IReadOnlyList<MagmellRawEvent>> ScrapeRawDocument(string rawHtml,
        CancellationToken cancellationToken)
    {
        using var document = await browsingContext.OpenAsync(req => req.Content(rawHtml), cancellationToken);
        var eventElements = document.QuerySelectorAll(".creator-detail-links__col");

        var result = eventElements.Select(e =>
            {
                var title = e.QuerySelector("h2")?.TextContent;
                var description = e.QuerySelector("p")?.TextContent;
                var linkElement = e.QuerySelector("a");
                var linkUrl = linkElement?.Attributes["href"]?.Value;

                return new { title = title, description = description, linkUrl = linkUrl };
            })
            .Select(e =>
            {
                if (e.title is null)
                {
                    return null;
                }

                var fullMatch = EventNameFullRegex().Match(e.title);
                var partialMatch = EventNameRegex().Match(e.title);

                if (fullMatch is { Length: 0 } || partialMatch is { Length: 0 })
                {
                    return null;
                }

                var date = partialMatch.Groups["date"].Value;
                var title = partialMatch.Groups["title"].Value;
                var place = fullMatch is { Length: 0 } ? null : fullMatch.Groups["place"].Value;

                return new MagmellRawEvent
                {
                    Date = date,
                    Title = title,
                    Place = place,
                    LinkTo = e.linkUrl,
                    Description = e.description
                };
            })
            .NotNull()
            .ToArray();

        return result;
    }

    private sealed class MagmellRawEvent
    {
        public required string Date { get; init; }
        public required string Title { get; init; }
        public required string? Place { get; init; }
        public required string? LinkTo { get; init; }
        public required string? Description { get; init; }
    }
}
