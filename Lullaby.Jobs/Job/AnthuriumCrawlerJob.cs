namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;
using Services.Crawler.Events;

public class AnthuriumCrawlerJob(
    IGroupCrawlerService groupCrawlerService,
    Anthurium anthurium,
    ICleanupOldScheduledEventsService cleanupOldScheduledEventsService
) : BaseCrawlerJob(groupCrawlerService, cleanupOldScheduledEventsService)
{
    public static string JobKey => "AnthuriumCrawlerJob";

    protected override IGroup TargetGroup => anthurium;
}
