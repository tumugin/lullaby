namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class YolozCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService,
    Yoloz yoloz
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public static readonly string JobKey = "YolozCrawlerJob";
    protected override IGroup TargetGroup => yoloz;
}
