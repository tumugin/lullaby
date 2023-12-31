namespace Lullaby.Job;

using Crawler;
using Groups;

public class AosekaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "AosekaCrawlerJob";

    private readonly Aoseka aoseka;

    public AosekaCrawlerJob(
        IGroupCrawler groupCrawler,
        Aoseka aoseka
    ) : base(groupCrawler) =>
        this.aoseka = aoseka;

    protected override IGroup TargetGroup => this.aoseka;


}
