namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class YosugalaCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    Yosugala yosugala,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public const string JobKey = "YosugalaCrawlerJob";

    protected override IGroup TargetGroup => yosugala;
}
