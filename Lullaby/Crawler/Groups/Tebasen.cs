namespace Lullaby.Crawler.Groups;

using Events;
using Scraper.Tebasen;

public class Tebasen : BaseGroup
{
    public const string GroupKeyConstant = "tebasen";
    public const string CrawlCronConstant = "0 0 * ? * * *";

    public override string GroupKey => GroupKeyConstant;
    public override string GroupName => "手羽先センセーション";
    public override string CrawlCron => CrawlCronConstant;

    private readonly TebasenSchedulePageScraper tebasenSchedulePageScraper;

    public Tebasen(TebasenSchedulePageScraper tebasenSchedulePageScraper) =>
        this.tebasenSchedulePageScraper = tebasenSchedulePageScraper;

    protected override Task<IReadOnlyList<GroupEvent>> GetEvents(CancellationToken cancellationToken) =>
        this.tebasenSchedulePageScraper.ScrapeAsync(cancellationToken);
}
