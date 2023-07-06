namespace Lullaby.Job;

using Crawler;
using Groups;

public class TenhanaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "TenhanaCrawlerJob";

    private readonly Tenhana tenhana;

    protected override IGroup TargetGroup => this.tenhana;

    public TenhanaCrawlerJob(IGroupCrawler groupCrawler, Tenhana tenhana) : base(groupCrawler) =>
        this.tenhana = tenhana;
}
