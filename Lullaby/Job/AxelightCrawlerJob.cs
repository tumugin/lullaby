namespace Lullaby.Job;

using Common.Groups;
using Crawler;

public class AxelightCrawlerJob : BaseCrawlerJob
{
    public static readonly string JobKey = "AxelightCrawlerJob";

    private readonly Axelight axelight;

    public AxelightCrawlerJob(IGroupCrawler groupCrawler, Axelight axelight) : base(groupCrawler) =>
        this.axelight = axelight;

    protected override IGroup TargetGroup => this.axelight;
}
