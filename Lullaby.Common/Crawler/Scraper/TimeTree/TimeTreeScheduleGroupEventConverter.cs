namespace Lullaby.Common.Crawler.Scraper.TimeTree;

using Events;

public class TimeTreeScheduleGroupEventConverter : ITimeTreeScheduleGroupEventConverter
{
    private readonly IEventTypeDetector eventTypeDetector;

    public TimeTreeScheduleGroupEventConverter(IEventTypeDetector eventTypeDetector) =>
        this.eventTypeDetector = eventTypeDetector;

    public GroupEvent Convert(TimeTreeApiResult.TimeTreeSchedule timeTreeSchedule)
    {
        var isAllDayEvent = timeTreeSchedule switch
        {
            {
                StartAt: { Hour: 0, Minute: 0, Second: 0 }, EndAt: { Hour: 0, Minute: 0, Second: 0 }
            } => true,
            _ => false
        };

        return new GroupEvent
        {
            EventName = timeTreeSchedule.Title,
            EventPlace = timeTreeSchedule.LocationName,
            EventDescription = timeTreeSchedule.Description,
            EventType = this.eventTypeDetector.DetectEventTypeByTitle(timeTreeSchedule.Title),
            EventDateTime = isAllDayEvent
                ? new UnDetailedEventDateTime
                {
                    EventStartDate = timeTreeSchedule.StartAt,
                    // NOTE: All day event from TimeTree API is like "Wed Jul 05 2023 23:59:59 GMT+0000" so we need to add 1 second to the end date.
                    // All day event stored in this application ends at 00:00:00 of the next day.
                    EventEndDate = timeTreeSchedule.UntilAt.AddSeconds(1)
                }
                : new DetailedEventDateTime
                {
                    EventStartDateTime = timeTreeSchedule.StartAt, EventEndDateTime = timeTreeSchedule.UntilAt
                }
        };
    }
}
