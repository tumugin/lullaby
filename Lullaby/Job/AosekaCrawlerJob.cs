namespace Lullaby.Job;

using Crawler.Groups;
using RestSharp;
using Services.Events;

public class AosekaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "AosekaCrawlerJob";

    protected override BaseGroup TargetGroup => this.aoseka;

    private readonly Aoseka aoseka;

    public AosekaCrawlerJob(
        IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService,
        Aoseka aoseka
    ) : base(
        addEventByGroupEventService,
        findDuplicateEventService,
        updateEventByGroupEventService
    ) =>
        this.aoseka = aoseka;
}
