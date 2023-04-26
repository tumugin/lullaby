namespace Lullaby.Job;

using Crawler;
using Groups;

public class OssCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "OSSCrawlerJob";

    private readonly Oss oss;

    public OssCrawlerJob(
        IGroupCrawler groupCrawler,
        Oss oss
    ) : base(groupCrawler) =>
        this.oss = oss;

    protected override IGroup TargetGroup => this.oss;
}
