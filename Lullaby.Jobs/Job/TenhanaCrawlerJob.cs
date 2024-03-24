namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class TenhanaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "TenhanaCrawlerJob";

    private readonly Tenhana tenhana;

    protected override IGroup TargetGroup => this.tenhana;

    public TenhanaCrawlerJob(IGroupCrawlerService groupCrawlerService, Tenhana tenhana) : base(groupCrawlerService) =>
        this.tenhana = tenhana;
}
