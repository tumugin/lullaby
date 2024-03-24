namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class YosugalaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "YosugalaCrawlerJob";

    private readonly Yosugala yosugala;

    public YosugalaCrawlerJob(
        IGroupCrawlerService groupCrawlerService,
        Yosugala yosugala
    ) : base(groupCrawlerService) =>
        this.yosugala = yosugala;

    protected override IGroup TargetGroup => this.yosugala;
}
