namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class NarlowCrawlerJob : BaseCrawlerJob
{
    private readonly Narlow narlow;
    public static string JobKey => "NarlowCrawlerJob";

    public NarlowCrawlerJob(IGroupCrawlerService groupCrawlerService, Narlow narlow) : base(groupCrawlerService)
        => this.narlow = narlow;

    protected override IGroup TargetGroup => this.narlow;
}
