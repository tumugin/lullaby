namespace Lullaby.Common.Crawler.Scraper.Magmell;

using System.Text.Json;
using System.Text.RegularExpressions;
using AngleSharp;
using Events;
using Groups;
using Litlink;
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
        var jsonElement = document.QuerySelector("#__NEXT_DATA__") ??
                          throw new InvalidDataException("data element was not found");
        var parsedJson = JsonSerializer.Deserialize<LitlinkJson.Root>(jsonElement.TextContent) ??
                         throw new JsonException("Failed to parse JSON");

        var result = parsedJson.Props?.PageProps.Profile.ProfileLinks
            .Where(e => e.ButtonLink is not null)
            .NotNull()
            .Select(e => new { e.ButtonLink!.Title, e.ButtonLink!.Description, LinkUrl = e.ButtonLink!.Url })
            .Select(e =>
            {
                if (e.Title is null)
                {
                    return null;
                }

                var fullMatch = EventNameFullRegex().Match(e.Title);
                var partialMatch = EventNameRegex().Match(e.Title);

                if (fullMatch is { Length: 0 } && partialMatch is { Length: 0 })
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
                    LinkTo = e.LinkUrl,
                    Description = e.Description
                };
            })
            .NotNull()
            .ToArray();

        if (result is null)
        {
            throw new InvalidDataException("Required properties are missing in the JSON object.");
        }

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
