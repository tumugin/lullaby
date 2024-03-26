namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class TenhanaCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    Tenhana tenhana,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public const string JobKey = "TenhanaCrawlerJob";

    protected override IGroup TargetGroup => tenhana;
}
