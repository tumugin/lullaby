namespace Lullaby.Jobs.Services.Crawler;

using Common.Crawler.Events;
using Common.Crawler.Scraper;
using Common.Groups;
using Database.DbContext;
using Events;

public class GroupCrawlerService : IGroupCrawlerService
{
    private readonly IAddEventByGroupEventService addEventByGroupEventService;
    private readonly IFindDuplicateEventService findDuplicateEventService;
    private readonly IEnumerable<ISchedulePageScraper> schedulePageScrapers;
    private readonly IUpdateEventByGroupEventService updateEventByGroupEventService;
    private readonly LullabyContext lullabyContext;

    public GroupCrawlerService(
        IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService,
        IEnumerable<ISchedulePageScraper> schedulePageScrapers,
        LullabyContext lullabyContext
    )
    {
        this.addEventByGroupEventService = addEventByGroupEventService;
        this.findDuplicateEventService = findDuplicateEventService;
        this.updateEventByGroupEventService = updateEventByGroupEventService;
        this.schedulePageScrapers = schedulePageScrapers;
        this.lullabyContext = lullabyContext;
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

        await using var transaction = await this.lullabyContext.Database.BeginTransactionAsync(cancellationToken);

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

        await transaction.CommitAsync(cancellationToken);
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
