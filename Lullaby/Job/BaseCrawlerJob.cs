namespace Lullaby.Job;

using Crawler;
using Groups;
using Quartz;

public abstract class BaseCrawlerJob : IJob
{
    private readonly IGroupCrawler groupCrawler;

    protected BaseCrawlerJob(IGroupCrawler groupCrawler) => this.groupCrawler = groupCrawler;
    protected abstract IGroup TargetGroup { get; }

    public virtual async Task Execute(IJobExecutionContext context) =>
        await this.groupCrawler.GetAndUpdateSavedEvents(this.TargetGroup, context.CancellationToken);
}
