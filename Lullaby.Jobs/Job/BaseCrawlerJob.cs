namespace Lullaby.Jobs.Job;

using Common.Groups;
using Hangfire;
using Services.Crawler;
using Services.Crawler.Events;

public abstract class BaseCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService
)
{
    protected abstract IGroup TargetGroup { get; }

    [AutomaticRetry(Attempts = 3)]
    public virtual async Task Execute(CancellationToken cancellationToken = default)
    {
        await groupCrawlerService.GetAndUpdateSavedEvents(this.TargetGroup, cancellationToken);
        await cleanupOldScheduledEventsService.ExecuteAsync(this.TargetGroup, cancellationToken);
    }
}
