namespace Lullaby.Job;

using Crawler.Groups;
using RestSharp;
using Services.Events;

public class OSSCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "OSSCrawlerJob";

    public OSSCrawlerJob(AddEventByGroupEventService addEventByGroupEventService,
        FindDuplicateEventService findDuplicateEventService,
        UpdateEventByGroupEventService updateEventByGroupEventService, RestClient restClient) : base(
        addEventByGroupEventService, findDuplicateEventService, updateEventByGroupEventService, restClient)
    {
    }

    protected override BaseGroup TargetGroup => new OSS();
}
