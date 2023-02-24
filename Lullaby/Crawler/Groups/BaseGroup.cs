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

    protected abstract Task<IEnumerable<GroupEvent>> GetEvents(
        RestClient restClient,
        CancellationToken cancellationToken
    );

    public async Task GetAndUpdateSavedEvents(
        IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService,
        RestClient restClient,
        CancellationToken cancellationToken
    )
    {
        var groupEvents = await this.GetEvents(restClient, cancellationToken);
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
            .Execute(findDuplicateEventsQuery, cancellationToken);

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
                await updateEventByGroupEventService.Execute(duplicateEvent, groupEvent, cancellationToken);
            }
            else
            {
                await addEventByGroupEventService.Execute(this.GroupKey, groupEvent, cancellationToken);
            }
        }
    }
}
