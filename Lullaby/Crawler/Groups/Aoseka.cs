namespace Lullaby.Crawler.Groups;

using Events;
using RestSharp;
using Scraper.Aoseka;

public class Aoseka : BaseGroup
{
    public const string GroupKeyConstant = "aoseka";

    public override string GroupKey => GroupKeyConstant;

    public override string GroupName => "群青の世界";

    // every hour
    public override string CrawlCron => "0 0 * ? * * *";

    protected override Task<IEnumerable<GroupEvent>> GetEvents(
        RestClient restClient,
        CancellationToken cancellationToken
    )
    {
        var aosekaScraper = new AosekaSchedulePageScraper(restClient);
        return aosekaScraper.ScrapeAsync(cancellationToken);
    }
}
