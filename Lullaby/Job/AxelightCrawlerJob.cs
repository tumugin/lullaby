namespace Lullaby.Job;

using Crawler;
using Groups;

public class AxelightCrawlerJob : BaseCrawlerJob
{
    public static readonly string JobKey = "AxelightCrawlerJob";

    private readonly Axelight axelight;

    public AxelightCrawlerJob(IGroupCrawler groupCrawler, Axelight axelight) : base(groupCrawler) =>
        this.axelight = axelight;

    protected override IGroup TargetGroup => this.axelight;
}
