namespace Lullaby.Job;

using Crawler.Groups;
using RestSharp;
using Services.Event;

public class YosugalaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "YosugalaCrawlerJob";

    public YosugalaCrawlerJob(
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

    protected override BaseGroup TargetGroup => new Yosugala();
}
