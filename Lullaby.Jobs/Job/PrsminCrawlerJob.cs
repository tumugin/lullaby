namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class PrsminCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    Prsmin prsmin,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public static readonly string JobKey = "PrsminCrawlerJob";

    protected override IGroup TargetGroup => prsmin;
}
