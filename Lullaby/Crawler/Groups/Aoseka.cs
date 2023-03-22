namespace Lullaby.Crawler.Groups;

using Events;
using Scraper.Aoseka;

public class Aoseka : BaseGroup
{
    public const string GroupKeyConstant = "aoseka";
    public const string CrawlCronConstant = "0 0 * ? * * *";

    public override string GroupKey => GroupKeyConstant;

    public override string GroupName => "群青の世界";

    // every hour
    public override string CrawlCron => CrawlCronConstant;

    private readonly AosekaSchedulePageScraper aosekaSchedulePageScraper;

    public Aoseka(AosekaSchedulePageScraper aosekaSchedulePageScraper) =>
        this.aosekaSchedulePageScraper = aosekaSchedulePageScraper;

    protected override Task<IReadOnlyList<GroupEvent>> GetEvents(CancellationToken cancellationToken) =>
        this.aosekaSchedulePageScraper.ScrapeAsync(cancellationToken);
}
