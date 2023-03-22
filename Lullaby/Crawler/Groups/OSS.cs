namespace Lullaby.Crawler.Groups;

using Events;
using Scraper.OSS;

public class OSS : BaseGroup
{
    public const string GroupKeyConstant = "oss";
    public const string CrawlCronConstant = "0 0 * ? * * *";
    public override string GroupKey => GroupKeyConstant;
    public override string GroupName => "On the treat Super Season";
    public override string CrawlCron => CrawlCronConstant;

    private readonly OSSSchedulePageScraper ossSchedulePageScraper;

    public OSS(OSSSchedulePageScraper ossSchedulePageScraper) => this.ossSchedulePageScraper = ossSchedulePageScraper;

    protected override Task<IReadOnlyList<GroupEvent>> GetEvents(CancellationToken cancellationToken) =>
        this.ossSchedulePageScraper.ScrapeAsync(cancellationToken);
}
