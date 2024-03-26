namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class TebasenCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    Tebasen tebasen,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public static readonly string JobKey = "TebasenCrawlerJob";

    protected override IGroup TargetGroup => tebasen;
}
