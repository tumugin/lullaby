namespace Lullaby.Job;

using Crawler;
using Groups;

public class PrsminCrawlerJob : BaseCrawlerJob
{
    public static readonly string JobKey = "PrsminCrawlerJob";

    private readonly Prsmin prsmin;

    public PrsminCrawlerJob(IGroupCrawler groupCrawler, Prsmin prsmin) : base(groupCrawler) => this.prsmin = prsmin;

    protected override IGroup TargetGroup => this.prsmin;
}
