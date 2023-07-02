namespace Lullaby.Crawler.Scraper.TimeTree;

using Events;

public class TimeTreeScheduleGroupEventConverter
{
    private readonly IEventTypeDetector eventTypeDetector;

    public TimeTreeScheduleGroupEventConverter(IEventTypeDetector eventTypeDetector) =>
        this.eventTypeDetector = eventTypeDetector;

    public GroupEvent Convert(TimeTreeApiResult.TimeTreeSchedule timeTreeSchedule) =>
        new()
        {
            EventName = timeTreeSchedule.Title,
            EventPlace = timeTreeSchedule.LocationName,
            EventDescription = timeTreeSchedule.Overview,
            EventType = this.eventTypeDetector.DetectEventTypeByTitle(timeTreeSchedule.Title),
            EventDateTime = timeTreeSchedule.IsAllDayEvent
                ? new UnDetailedEventDateTime
                {
                    EventStartDate = timeTreeSchedule.StartAt, EventEndDate = timeTreeSchedule.EndAt
                }
                : new DetailedEventDateTime
                {
                    EventStartDateTime = timeTreeSchedule.StartAt, EventEndDateTime = timeTreeSchedule.EndAt
                }
        };
}
