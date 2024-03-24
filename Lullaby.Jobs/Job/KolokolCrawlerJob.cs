namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class KolokolCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "KolokolCrawlerJob";

    private readonly Kolokol kolokol;

    public KolokolCrawlerJob(
        IGroupCrawlerService groupCrawlerService,
        Kolokol kolokol
    ) : base(groupCrawlerService) =>
        this.kolokol = kolokol;

    protected override IGroup TargetGroup => this.kolokol;
}
