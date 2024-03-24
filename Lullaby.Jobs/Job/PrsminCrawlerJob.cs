namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class PrsminCrawlerJob : BaseCrawlerJob
{
    public static readonly string JobKey = "PrsminCrawlerJob";

    private readonly Prsmin prsmin;

    public PrsminCrawlerJob(IGroupCrawlerService groupCrawlerService, Prsmin prsmin) : base(groupCrawlerService) => this.prsmin = prsmin;

    protected override IGroup TargetGroup => this.prsmin;
}
