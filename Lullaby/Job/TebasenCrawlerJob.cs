namespace Lullaby.Job;

using Common.Groups;
using Crawler;

public class TebasenCrawlerJob : BaseCrawlerJob
{
    public static readonly string JobKey = "TebasenCrawlerJob";

    private readonly Tebasen tebasen;

    public TebasenCrawlerJob(
        IGroupCrawler groupCrawler,
        Tebasen tebasen
    ) : base(groupCrawler) =>
        this.tebasen = tebasen;

    protected override IGroup TargetGroup => this.tebasen;
}
