namespace Lullaby.Job;

using Crawler.Groups;
using Quartz;
using RestSharp;
using Services.Events;

public abstract class BaseCrawlerJob : IJob
{
    protected AddEventByGroupEventService AddEventByGroupEventService { get; }
    protected FindDuplicateEventService FindDuplicateEventService { get; }
    protected UpdateEventByGroupEventService UpdateEventByGroupEventService { get; }
    protected RestClient RestClient { get; }

    protected abstract BaseGroup TargetGroup { get; }

    protected BaseCrawlerJob(
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

    public virtual async Task Execute(IJobExecutionContext context) =>
        await this.TargetGroup.GetAndUpdateSavedEvents(
            this.AddEventByGroupEventService,
            this.FindDuplicateEventService,
            this.UpdateEventByGroupEventService,
            this.RestClient
        );
}
