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
            var targetDuplicateEvents = duplicateEvents.Where(d =>
                d.GroupKey == group.GroupKey &&
                d.EventName.Trim() == groupEvent.EventName.Trim() &&
                d.EventStarts == groupEvent.EventDateTime.EventStartDateTimeOffset &&
                d.EventEnds == groupEvent.EventDateTime.EventEndDateTimeOffset
            ).ToArray();

            if (targetDuplicateEvents.Length > 0)
            {
                await this.updateEventByGroupEventService.Execute(
                    targetDuplicateEvents.First(),
                    groupEvent,
                    cancellationToken
                );

                // Find duplicate events and update only the first one, remove others
                foreach (var duplicateEvent in targetDuplicateEvents.Skip(1))
                {
                    this.lullabyContext.Events.Remove(duplicateEvent);
                    await this.lullabyContext.SaveChangesAsync(cancellationToken);
                }
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
