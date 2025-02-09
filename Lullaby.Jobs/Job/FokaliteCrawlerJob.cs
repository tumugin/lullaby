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
}
