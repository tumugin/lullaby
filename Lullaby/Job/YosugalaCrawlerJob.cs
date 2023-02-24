namespace Lullaby.Job;

using Crawler.Groups;
using RestSharp;
using Services.Events;

public class YosugalaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "YosugalaCrawlerJob";

    public YosugalaCrawlerJob(
        IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService,
        RestClient restClient
    ) : base(
        addEventByGroupEventService,
        findDuplicateEventService,
        updateEventByGroupEventService,
        restClient
    )
    {
    }

    protected override BaseGroup TargetGroup => new Yosugala();
}
