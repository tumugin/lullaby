namespace Lullaby.Common.Crawler.Scraper;

using Events;

public interface ISchedulePageScraper
{
    public Type TargetGroup { get; }
    public Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken);
}
