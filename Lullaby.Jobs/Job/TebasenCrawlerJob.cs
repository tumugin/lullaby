namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class TebasenCrawlerJob : BaseCrawlerJob
{
    public static readonly string JobKey = "TebasenCrawlerJob";

    private readonly Tebasen tebasen;

    public TebasenCrawlerJob(
        IGroupCrawlerService groupCrawlerService,
        Tebasen tebasen
    ) : base(groupCrawlerService) =>
        this.tebasen = tebasen;

    protected override IGroup TargetGroup => this.tebasen;
}
