namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class NarlowCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    Narlow narlow,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public static string JobKey => "NarlowCrawlerJob";

    protected override IGroup TargetGroup => narlow;
}
