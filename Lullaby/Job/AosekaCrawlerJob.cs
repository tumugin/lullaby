namespace Lullaby.Job;

using Crawler.Groups;
using RestSharp;
using Services.Event;

public class AosekaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "AosekaCrawlerJob";

    protected override BaseGroup TargetGroup => new Aoseka();

    public AosekaCrawlerJob(
        AddEventByGroupEventService addEventByGroupEventService,
        FindDuplicateEventService findDuplicateEventService,
        UpdateEventByGroupEventService updateEventByGroupEventService,
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
