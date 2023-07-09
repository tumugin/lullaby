namespace Lullaby.Job;

using Crawler;
using Groups;

public abstract class BaseCrawlerJob
{
    private readonly IGroupCrawler groupCrawler;

    protected BaseCrawlerJob(IGroupCrawler groupCrawler) => this.groupCrawler = groupCrawler;
    protected abstract IGroup TargetGroup { get; }

    public async Task Execute(CancellationToken cancellationToken) =>
        await this.groupCrawler.GetAndUpdateSavedEvents(this.TargetGroup, cancellationToken);
}
