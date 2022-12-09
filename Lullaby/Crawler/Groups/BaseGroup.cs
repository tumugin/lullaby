namespace Lullaby.Crawler.Groups;

using Events;
using Services.Event;

public abstract class BaseGroup
{
    public abstract string GroupKey { get; }

    public abstract string GroupName { get; }

    public abstract string CrawlCron { get; }

    public abstract Task<IEnumerable<GroupEvent>> getEvents();

    public async Task getAndUpdateSavedEvents(
        AddEventByGroupEventService addEventByGroupEventService,
        FindDuplicateEventService findDuplicateEventService,
        UpdateEventByGroupEventService updateEventByGroupEventService
    )
    {
        var groupEvents = await this.getEvents();
        var findDuplicateEventsQuery = groupEvents
            .Select(v =>
                new FindDuplicateEventService.EventSearchQueryData()
                {
                    GroupKey = this.GroupKey,
                    EventName = v.EventName,
                    StartDateTime = v.EventDateTime switch
                    {
                        DetailedEventDateTime detailedEventDateTime => detailedEventDateTime.EventStartDateTime,
                        UnDetailedEventDateTime unDetailedEventDateTime => unDetailedEventDateTime.EventStartDate,
                        _ => throw new ArgumentException("EventDateTime is not a valid type"),
                    },
                    EndDateTime = v.EventDateTime switch
                    {
                        DetailedEventDateTime detailedEventDateTime => detailedEventDateTime.EventEndDateTime,
                        UnDetailedEventDateTime unDetailedEventDateTime => unDetailedEventDateTime.EventEndDate,
                        _ => throw new ArgumentException("EventDateTime is not a valid type"),
                    }
                }
            );

        var duplicateEvents = await findDuplicateEventService
            .Execute(findDuplicateEventsQuery);

        foreach (var groupEvent in groupEvents)
        {
            var duplicateEvent = duplicateEvents.FirstOrDefault(d =>
                d.GroupKey == this.GroupKey &&
                d.EventName == groupEvent.EventName &&
                d.EventStarts == groupEvent.EventDateTime switch
                {
                    DetailedEventDateTime detailedEventDateTime =>
                        detailedEventDateTime.EventStartDateTime,
                    UnDetailedEventDateTime unDetailedEventDateTime
                        => unDetailedEventDateTime.EventStartDate,
                    _ => throw new ArgumentException(
                        "EventDateTime is not a valid type"),
                } &&
                d.EventEnds == groupEvent.EventDateTime switch
                {
                    DetailedEventDateTime detailedEventDateTime =>
                        detailedEventDateTime.EventEndDateTime,
                    UnDetailedEventDateTime unDetailedEventDateTime
                        => unDetailedEventDateTime.EventEndDate,
                    _ => throw new ArgumentException(
                        "EventDateTime is not a valid type"),
                }
            );
            if (duplicateEvent != null)
            {
                await updateEventByGroupEventService.Execute(duplicateEvent, groupEvent);
            }
            else
            {
                await addEventByGroupEventService.Execute(this.GroupKey, groupEvent);
            }
        }
    }
}
