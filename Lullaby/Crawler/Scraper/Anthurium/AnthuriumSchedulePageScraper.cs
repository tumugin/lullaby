namespace Lullaby.Crawler.Scraper.Anthurium;

using AngleSharp.Html.Parser;
using ApiClient;
using Events;
using Groups;

public class AnthuriumSchedulePageScraper : ISchedulePageScraper
{
    private readonly IAnthuriumApiClient anthuriumApiClient;
    private readonly IEventTypeDetector eventTypeDetector;
    private readonly IHtmlParser htmlParser;

    public AnthuriumSchedulePageScraper(
        IAnthuriumApiClient anthuriumApiClient,
        IEventTypeDetector eventTypeDetector,
        IHtmlParser htmlParser
    )
    {
        this.anthuriumApiClient = anthuriumApiClient;
        this.eventTypeDetector = eventTypeDetector;
        this.htmlParser = htmlParser;
    }

    public Type TargetGroup => typeof(Anthurium);

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var now = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo"));
        var startMonth = now.AddMonths(-3);
        var endMonth = now.AddMonths(6);
        // Split date range by 30 days and send request
        var dateRanges = SplitDateRange(startMonth, endMonth, 30);
        var tasks = dateRanges.Select(dateRange =>
            this.anthuriumApiClient.GetSchedules(dateRange.Start, dateRange.End, cancellationToken)
        );
        var results = await Task.WhenAll(tasks);

        return results
            .SelectMany(x => x)
            .Select(v => new GroupEvent
            {
                EventName = v.EventName,
                EventPlace = null,
                EventDateTime =
                    v.IsAllDay
                        // NOTE: All day event from the API is like "Wed Jul 05 2023 23:59:59 GMT+0000" so we need to add 1 second to the end date.
                        // All day event stored in this application ends at 00:00:00 of the next day.
                        ? new UnDetailedEventDateTime
                        {
                            EventStartDate = v.StartDate, EventEndDate = v.EndDate.AddSeconds(1)
                        }
                        : new DetailedEventDateTime { EventStartDateTime = v.StartDate, EventEndDateTime = v.EndDate },
                EventType = this.eventTypeDetector.DetectEventTypeByTitle(v.EventName),
                EventDescription = this.htmlParser.ParseDocument(v.EventDetail).TextContent
            })
            .ToArray();
    }

    private static List<DateTimeRange> SplitDateRange(DateTimeOffset start, DateTimeOffset end, int splitDays)
    {
        var dateRanges = new List<DateTimeRange>();
        var currentDate = start;
        while (currentDate < end)
        {
            var nextDate = currentDate.AddDays(splitDays);
            if (nextDate > end)
            {
                nextDate = end;
            }

            dateRanges.Add(new DateTimeRange { Start = currentDate, End = nextDate });
            currentDate = nextDate;
        }

        return dateRanges;
    }

    private sealed class DateTimeRange
    {
        public required DateTimeOffset Start { get; init; }
        public required DateTimeOffset End { get; init; }
    }
}
