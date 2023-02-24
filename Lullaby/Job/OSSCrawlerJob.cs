namespace Lullaby.Job;

using Crawler.Groups;
using RestSharp;
using Services.Events;

public class OSSCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "OSSCrawlerJob";

    public OSSCrawlerJob(
        IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService,
        RestClient restClient
    ) : base(
        addEventByGroupEventService, findDuplicateEventService, updateEventByGroupEventService, restClient)
    {
    }

    protected override BaseGroup TargetGroup => new OSS();
}
