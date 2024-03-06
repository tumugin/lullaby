namespace Lullaby.Job;

using Common.Groups;
using Crawler;

public class KolokolCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "KolokolCrawlerJob";

    private readonly Kolokol kolokol;

    public KolokolCrawlerJob(
        IGroupCrawler groupCrawler,
        Kolokol kolokol
    ) : base(groupCrawler) =>
        this.kolokol = kolokol;

    protected override IGroup TargetGroup => this.kolokol;
}
