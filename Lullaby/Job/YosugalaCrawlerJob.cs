namespace Lullaby.Job;

using Crawler.Groups;
using RestSharp;
using Services.Events;

public class YosugalaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "YosugalaCrawlerJob";

    private readonly Yosugala yosugala;

    public YosugalaCrawlerJob(
        IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService,
        Yosugala yosugala
    ) : base(
        addEventByGroupEventService,
        findDuplicateEventService,
        updateEventByGroupEventService
    ) =>
        this.yosugala = yosugala;

    protected override BaseGroup TargetGroup => this.yosugala;
}
