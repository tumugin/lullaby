namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class AxelightCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    Axelight axelight,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public static readonly string JobKey = "AxelightCrawlerJob";

    protected override IGroup TargetGroup => axelight;
}
