namespace Lullaby.Crawler;

using Events;
using Groups;
using Scraper;
using Services.Events;

public class GroupCrawler : IGroupCrawler
{
    private readonly IAddEventByGroupEventService addEventByGroupEventService;
    private readonly IFindDuplicateEventService findDuplicateEventService;
    private readonly IEnumerable<ISchedulePageScraper> schedulePageScrapers;
    private readonly IUpdateEventByGroupEventService updateEventByGroupEventService;

    public GroupCrawler(
        IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService,
        IEnumerable<ISchedulePageScraper> schedulePageScrapers
    )
    {
        this.addEventByGroupEventService = addEventByGroupEventService;
        this.findDuplicateEventService = findDuplicateEventService;
        this.updateEventByGroupEventService = updateEventByGroupEventService;
        this.schedulePageScrapers = schedulePageScrapers;
    }

    public async Task GetAndUpdateSavedEvents(
        IGroup group,
        CancellationToken cancellationToken
    )
    {
        var groupEvents = await this.GetEvents(group, cancellationToken);
        var findDuplicateEventsQuery = groupEvents
            .Select(v =>
                new IFindDuplicateEventService.EventSearchQueryData
                {
                    GroupKey = group.GroupKey,
                    EventName = v.EventName,
                    StartDateTime = v.EventDateTime.EventStartDateTimeOffset,
                    EndDateTime = v.EventDateTime.EventEndDateTimeOffset
                }
            );

        var duplicateEvents = await this.findDuplicateEventService
            .Execute(findDuplicateEventsQuery, cancellationToken);

        foreach (var groupEvent in groupEvents)
        {
            var duplicateEvent = duplicateEvents.FirstOrDefault(d =>
                d.GroupKey == group.GroupKey &&
                d.EventName == groupEvent.EventName &&
                d.EventStarts == groupEvent.EventDateTime.EventStartDateTimeOffset &&
                d.EventEnds == groupEvent.EventDateTime.EventEndDateTimeOffset
            );
            if (duplicateEvent != null)
            {
                await this.updateEventByGroupEventService.Execute(duplicateEvent, groupEvent, cancellationToken);
            }
            else
            {
                await this.addEventByGroupEventService.Execute(group.GroupKey, groupEvent, cancellationToken);
            }
        }
    }

    private Task<IReadOnlyList<GroupEvent>> GetEvents(
        IGroup group,
        CancellationToken cancellationToken
    )
    {
        var pageScraper = this.schedulePageScrapers.First(v => v.TargetGroup == group.GetType());
        return pageScraper.ScrapeAsync(cancellationToken);
    }
}
