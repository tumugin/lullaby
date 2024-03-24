namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class AosekaCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    Aoseka aoseka,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public const string JobKey = "AosekaCrawlerJob";

    protected override IGroup TargetGroup => aoseka;

    /**
     *  disable for now because group has been suspended
     */
    public override Task Execute(CancellationToken cancellationToken = default) => Task.CompletedTask;
}
