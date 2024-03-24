namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class TenrinCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    Tenrin tenrin,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public const string JobKey = "TenrinCrawlerJob";

    protected override IGroup TargetGroup => tenrin;
}
