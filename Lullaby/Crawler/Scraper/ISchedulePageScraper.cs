namespace Lullaby.Crawler.Scraper;

using Events;

public interface ISchedulePageScraper
{
    public Type TargetGroup { get; }
    public Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken);
}
