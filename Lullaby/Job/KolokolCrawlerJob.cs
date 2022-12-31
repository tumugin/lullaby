namespace Lullaby.Job;

using Crawler.Groups;
using Quartz;
using RestSharp;
using Services.Event;

public class KolokolCrawlerJob : BaseCrawlerJob
{
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

    public override async Task Execute(IJobExecutionContext context)
    {
        var kolokol = new Kolokol();
        await kolokol.GetAndUpdateSavedEvents(
            this.AddEventByGroupEventService,
            this.FindDuplicateEventService,
            this.UpdateEventByGroupEventService,
            this.RestClient
        );
    }
}
