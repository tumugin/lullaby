namespace Lullaby.Crawler.Groups;

using Events;
using Scraper.Kolokol;

public class Kolokol : BaseGroup
{
    public const string GroupKeyConstant = "kolokol";
    public const string CrawlCronConstant = "0 0 * ? * * *";

    public override string GroupKey => GroupKeyConstant;

    public override string GroupName => "Kolokol";

    // every hour
    public override string CrawlCron => CrawlCronConstant;

    private readonly KolokolSchedulePageScraper kolokolSchedulePageScraper;

    public Kolokol(KolokolSchedulePageScraper kolokolSchedulePageScraper) =>
        this.kolokolSchedulePageScraper = kolokolSchedulePageScraper;

    protected override Task<IReadOnlyList<GroupEvent>> GetEvents(CancellationToken cancellationToken) =>
        this.kolokolSchedulePageScraper.ScrapeAsync(cancellationToken);
}
