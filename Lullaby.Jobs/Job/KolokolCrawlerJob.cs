namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class KolokolCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    Kolokol kolokol,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public const string JobKey = "KolokolCrawlerJob";

    protected override IGroup TargetGroup => kolokol;
}
