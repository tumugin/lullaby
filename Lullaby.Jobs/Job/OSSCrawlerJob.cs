namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class OssCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "OSSCrawlerJob";

    private readonly Oss oss;

    public OssCrawlerJob(
        IGroupCrawlerService groupCrawlerService,
        Oss oss
    ) : base(groupCrawlerService) =>
        this.oss = oss;

    protected override IGroup TargetGroup => this.oss;

    /**
     *  disable for now because group has been suspended
     */
    public override Task Execute(CancellationToken cancellationToken = default) => Task.CompletedTask;
}
