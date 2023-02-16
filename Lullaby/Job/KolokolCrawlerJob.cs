namespace Lullaby.Job;

using Crawler.Groups;
using RestSharp;
using Services.Events;

public class KolokolCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "KolokolCrawlerJob";

    protected override BaseGroup TargetGroup => new Kolokol();

    public KolokolCrawlerJob(
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
