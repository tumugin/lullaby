namespace Lullaby.Job;

using Crawler.Groups;
using Services.Events;

public class KolokolCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "KolokolCrawlerJob";

    protected override BaseGroup TargetGroup => this.kolokol;

    private readonly Kolokol kolokol;

    public KolokolCrawlerJob(
        IAddEventByGroupEventService addEventByGroupEventService,
        IFindDuplicateEventService findDuplicateEventService,
        IUpdateEventByGroupEventService updateEventByGroupEventService,
        Kolokol kolokol
    ) : base(
        addEventByGroupEventService,
        findDuplicateEventService,
        updateEventByGroupEventService
    ) =>
        this.kolokol = kolokol;
}
