namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class AxelightCrawlerJob : BaseCrawlerJob
{
    public static readonly string JobKey = "AxelightCrawlerJob";

    private readonly Axelight axelight;

    public AxelightCrawlerJob(IGroupCrawlerService groupCrawlerService, Axelight axelight) : base(groupCrawlerService) =>
        this.axelight = axelight;

    protected override IGroup TargetGroup => this.axelight;
}
