namespace Lullaby.Job;

using Common.Groups;
using Crawler;

public class TenhanaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "TenhanaCrawlerJob";

    private readonly Tenhana tenhana;

    protected override IGroup TargetGroup => this.tenhana;

    public TenhanaCrawlerJob(IGroupCrawler groupCrawler, Tenhana tenhana) : base(groupCrawler) =>
        this.tenhana = tenhana;
}
