namespace Lullaby.Job;

using Crawler;
using Groups;

public class YosugalaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "YosugalaCrawlerJob";

    private readonly Yosugala yosugala;

    public YosugalaCrawlerJob(
        IGroupCrawler groupCrawler,
        Yosugala yosugala
    ) : base(groupCrawler) =>
        this.yosugala = yosugala;

    protected override IGroup TargetGroup => this.yosugala;
}
