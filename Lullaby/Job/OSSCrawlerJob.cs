namespace Lullaby.Job;

using Crawler.Groups;
using Services.Events;

public class OssCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "OSSCrawlerJob";

    private readonly Oss oss;

    public OssCrawlerJob(
        IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService,
        Oss oss
    ) : base(
        addEventByGroupEventService, findDuplicateEventService, updateEventByGroupEventService
    ) =>
        this.oss = oss;

    protected override BaseGroup TargetGroup => this.oss;
}
