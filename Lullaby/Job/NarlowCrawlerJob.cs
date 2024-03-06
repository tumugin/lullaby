namespace Lullaby.Job;

using Common.Groups;
using Crawler;

public class NarlowCrawlerJob : BaseCrawlerJob
{
    private readonly Narlow narlow;
    public static string JobKey => "NarlowCrawlerJob";

    public NarlowCrawlerJob(IGroupCrawler groupCrawler, Narlow narlow) : base(groupCrawler)
        => this.narlow = narlow;

    protected override IGroup TargetGroup => this.narlow;
}
