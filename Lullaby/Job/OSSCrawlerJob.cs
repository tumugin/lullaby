namespace Lullaby.Job;

using Crawler.Groups;
using RestSharp;
using Services.Events;

public class OSSCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "OSSCrawlerJob";

    private readonly OSS oss;

    public OSSCrawlerJob(
        IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService, OSS oss
    ) : base(
        addEventByGroupEventService, findDuplicateEventService, updateEventByGroupEventService
    ) =>
        this.oss = oss;

    protected override BaseGroup TargetGroup => this.oss;
}
