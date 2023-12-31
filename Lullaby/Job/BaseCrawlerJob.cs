namespace Lullaby.Job;

using Crawler;
using Groups;
using Hangfire;

public abstract class BaseCrawlerJob
{
    private readonly IGroupCrawler groupCrawler;

    protected BaseCrawlerJob(IGroupCrawler groupCrawler) => this.groupCrawler = groupCrawler;
    protected abstract IGroup TargetGroup { get; }

    [AutomaticRetry(Attempts = 3)]
    public virtual async Task Execute(CancellationToken cancellationToken = default) =>
        await this.groupCrawler.GetAndUpdateSavedEvents(this.TargetGroup, cancellationToken);
}
