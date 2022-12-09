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
                    StartDateTime = v.EventDateTime.EventStartDateTimeOffset,
                    EndDateTime = v.EventDateTime.EventEndDateTimeOffset
                }
            );

        var duplicateEvents = await findDuplicateEventService
            .Execute(findDuplicateEventsQuery);

        foreach (var groupEvent in groupEvents)
        {
            var duplicateEvent = duplicateEvents.FirstOrDefault(d =>
                d.GroupKey == this.GroupKey &&
                d.EventName == groupEvent.EventName &&
                d.EventStarts == groupEvent.EventDateTime.EventStartDateTimeOffset &&
                d.EventEnds == groupEvent.EventDateTime.EventEndDateTimeOffset
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
