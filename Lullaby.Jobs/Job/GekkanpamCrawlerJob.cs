namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class GekkanpamCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService,
    Gekkanpam gekkanpam
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    protected override IGroup TargetGroup => gekkanpam;

    public static string JobKey => "GekkanpamCrawlerJob";
}
