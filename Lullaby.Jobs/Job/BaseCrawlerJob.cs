namespace Lullaby.Jobs.Job;

using Common.Groups;
using Hangfire;
using Services.Crawler;

public abstract class BaseCrawlerJob
{
    private readonly IGroupCrawlerService groupCrawlerService;

    protected BaseCrawlerJob(IGroupCrawlerService groupCrawlerService) => this.groupCrawlerService = groupCrawlerService;
    protected abstract IGroup TargetGroup { get; }

    [AutomaticRetry(Attempts = 3)]
    public virtual async Task Execute(CancellationToken cancellationToken = default) =>
        await this.groupCrawlerService.GetAndUpdateSavedEvents(this.TargetGroup, cancellationToken);
}
