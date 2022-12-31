namespace Lullaby.Job;

using Quartz;
using RestSharp;
using Services.Event;

public abstract class BaseCrawlerJob : IJob
{
    protected AddEventByGroupEventService AddEventByGroupEventService { get; }
    protected FindDuplicateEventService FindDuplicateEventService { get; }
    protected UpdateEventByGroupEventService UpdateEventByGroupEventService { get; }
    protected RestClient RestClient { get; }

    public BaseCrawlerJob(
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

    public abstract Task Execute(IJobExecutionContext context);
}
