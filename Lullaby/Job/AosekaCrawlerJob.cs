namespace Lullaby.Job;

using Crawler.Groups;
using RestSharp;
using Services.Events;

public class AosekaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "AosekaCrawlerJob";

    protected override BaseGroup TargetGroup => new Aoseka();

    public AosekaCrawlerJob(
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
}
