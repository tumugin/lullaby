namespace Lullaby.Job;

using Crawler.Groups;
using Quartz;
using RestSharp;
using Services.Event;

public class AosekaCrawlerJob : IJob
{
    private AddEventByGroupEventService AddEventByGroupEventService { get; }
    private FindDuplicateEventService FindDuplicateEventService { get; }
    private UpdateEventByGroupEventService UpdateEventByGroupEventService { get; }
    private RestClient RestClient { get; }

    public AosekaCrawlerJob(
        AddEventByGroupEventService addEventByGroupEventService,
        FindDuplicateEventService findDuplicateEventService,
        UpdateEventByGroupEventService updateEventByGroupEventService,
        RestClient restClient)
    {
        this.AddEventByGroupEventService = addEventByGroupEventService;
        this.FindDuplicateEventService = findDuplicateEventService;
        this.UpdateEventByGroupEventService = updateEventByGroupEventService;
        this.RestClient = restClient;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var aoseka = new Aoseka();
        await aoseka.GetAndUpdateSavedEvents(
            this.AddEventByGroupEventService,
            this.FindDuplicateEventService,
            this.UpdateEventByGroupEventService,
            this.RestClient
        );
    }
}
