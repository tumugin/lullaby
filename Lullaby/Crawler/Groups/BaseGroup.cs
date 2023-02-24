namespace Lullaby.Crawler.Groups;

using Events;
using RestSharp;
using Services.Events;

public abstract class BaseGroup
{
    public abstract string GroupKey { get; }

    public abstract string GroupName { get; }

    // see: https://www.freeformatter.com/cron-expression-generator-quartz.html
    // see: https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/crontriggers.html
    public abstract string CrawlCron { get; }

    public abstract Task<IEnumerable<GroupEvent>> GetEvents(RestClient restClient);

    public async Task GetAndUpdateSavedEvents(
        AddEventByGroupEventService addEventByGroupEventService,
        FindDuplicateEventService findDuplicateEventService,
        UpdateEventByGroupEventService updateEventByGroupEventService,
        RestClient restClient
    )
    {
        var groupEvents = await this.GetEvents(restClient);
        var findDuplicateEventsQuery = groupEvents
            .Select(v =>
                new IFindDuplicateEventService.EventSearchQueryData()
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
