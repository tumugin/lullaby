namespace Lullaby.Jobs.Job;

using Common.Groups;
using Services.Crawler;

public class AosekaCrawlerJob : BaseCrawlerJob
{
    public const string JobKey = "AosekaCrawlerJob";

    private readonly Aoseka aoseka;

    public AosekaCrawlerJob(
        IGroupCrawlerService groupCrawlerService,
        Aoseka aoseka
    ) : base(groupCrawlerService) =>
        this.aoseka = aoseka;

    protected override IGroup TargetGroup => this.aoseka;

    /**
     *  disable for now because group has been suspended
     */
    public override Task Execute(CancellationToken cancellationToken = default) => Task.CompletedTask;
}
