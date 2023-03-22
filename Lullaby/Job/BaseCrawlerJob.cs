namespace Lullaby.Job;

using Crawler.Groups;
using Quartz;
using Services.Events;

public abstract class BaseCrawlerJob : IJob
{
    private IAddEventByGroupEventService AddEventByGroupEventService { get; }
    private IFindDuplicateEventService FindDuplicateEventService { get; }
    private IUpdateEventByGroupEventService UpdateEventByGroupEventService { get; }
    protected abstract BaseGroup TargetGroup { get; }

    protected BaseCrawlerJob(
        IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService
    )
    {
        this.AddEventByGroupEventService = addEventByGroupEventService;
        this.FindDuplicateEventService = findDuplicateEventService;
        this.UpdateEventByGroupEventService = updateEventByGroupEventService;
    }

    public virtual async Task Execute(IJobExecutionContext context) =>
        await this.TargetGroup.GetAndUpdateSavedEvents(
            this.AddEventByGroupEventService,
            this.FindDuplicateEventService,
            this.UpdateEventByGroupEventService,
            context.CancellationToken
        );
}
