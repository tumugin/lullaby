namespace Lullaby.Crawler.Groups;

using Events;
using Scraper.Yosugala;

public class Yosugala : BaseGroup
{
    public const string GroupKeyConstant = "yosugala";
    public const string CrawlCronConstant = "0 0 * ? * * *";

    public override string GroupKey => GroupKeyConstant;

    public override string GroupName => "yosugala";

    // every hour
    public override string CrawlCron => CrawlCronConstant;

    private readonly YosugalaSchedulePageScraper yosugalaSchedulePageScraper;

    public Yosugala(YosugalaSchedulePageScraper yosugalaSchedulePageScraper) =>
        this.yosugalaSchedulePageScraper = yosugalaSchedulePageScraper;

    protected override Task<IReadOnlyList<GroupEvent>> GetEvents(CancellationToken cancellationToken) =>
        this.yosugalaSchedulePageScraper.ScrapeAsync(cancellationToken);
}
