namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class FokaliteCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService,
    Fokalite fokalite
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public const string JobKey = "FokaliteCrawlerJob";

    protected override IGroup TargetGroup => fokalite;

    /// <summary>
    /// disable for now because group calendar has moved to GCal
    /// </summary>
    public override Task Execute(CancellationToken cancellationToken = default) => Task.CompletedTask;
}
