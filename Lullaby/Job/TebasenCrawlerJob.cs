namespace Lullaby.Job;

using Crawler.Groups;
using Services.Events;

public class TebasenCrawlerJob : BaseCrawlerJob
{
    public static readonly string JobKey = "TebasenCrawlerJob";

    private readonly Tebasen tebasen;

    public TebasenCrawlerJob(IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService,
        Tebasen tebasen) : base(addEventByGroupEventService,
        findDuplicateEventService, updateEventByGroupEventService) =>
        this.tebasen = tebasen;

    protected override BaseGroup TargetGroup => this.tebasen;
}
