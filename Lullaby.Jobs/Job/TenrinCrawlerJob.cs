namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class TenrinCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "TenrinCrawlerJob";

    private readonly Tenrin tenrin;
    protected override IGroup TargetGroup => this.tenrin;

    public TenrinCrawlerJob(IGroupCrawlerService groupCrawlerService, Tenrin tenrin) : base(groupCrawlerService)
        => this.tenrin = tenrin;
}
