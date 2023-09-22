namespace Lullaby.Job;

using Crawler;
using Groups;

public class TenrinCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "TenrinCrawlerJob";

    private readonly Tenrin tenrin;
    protected override IGroup TargetGroup => this.tenrin;

    public TenrinCrawlerJob(IGroupCrawler groupCrawler, Tenrin tenrin) : base(groupCrawler)
        => this.tenrin = tenrin;
}
