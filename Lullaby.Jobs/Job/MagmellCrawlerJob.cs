namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class MagmellCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService,
    Magmell magmell
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public static string JobKey => "MagmellCrawlerJob";

    protected override IGroup TargetGroup => magmell;
}
