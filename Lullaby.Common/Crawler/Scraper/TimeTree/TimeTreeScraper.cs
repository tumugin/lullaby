namespace Lullaby.Common.Crawler.Scraper.TimeTree;

using Events;

public abstract class TimeTreeScraper
{
    public abstract string TimeTreePublicCalendarId { get; }

    private readonly ITimeTreeApiClient timeTreeApiClient;
    private readonly ITimeTreeScheduleGroupEventConverter timeTreeScheduleGroupEventConverter;

    protected TimeTreeScraper(ITimeTreeApiClient timeTreeApiClient,
        ITimeTreeScheduleGroupEventConverter timeTreeScheduleGroupEventConverter)
    {
        this.timeTreeApiClient = timeTreeApiClient;
        this.timeTreeScheduleGroupEventConverter = timeTreeScheduleGroupEventConverter;
    }

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var scheduleFetchStartDateTime = DateTimeOffset.UtcNow.AddMonths(-1);
        var scheduleFetchEndDateTime = DateTimeOffset.UtcNow.AddMonths(3);

        string? cursor = null;
        var timeTreeSchedules = new List<TimeTreeApiResult.TimeTreeSchedule>();

        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await this.timeTreeApiClient.GetEventsAsync(
                this.TimeTreePublicCalendarId,
                scheduleFetchStartDateTime,
                scheduleFetchEndDateTime,
                cursor,
                cancellationToken
            );

            timeTreeSchedules.AddRange(result.Schedules);
            cursor = result.NextPageCursor;

            if (!result.HasNextPage)
            {
                break;
            }
        }

        return timeTreeSchedules
            .Select(v => this.timeTreeScheduleGroupEventConverter.Convert(v))
            .ToArray();
    }
}
