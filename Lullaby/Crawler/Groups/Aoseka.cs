namespace Lullaby.Crawler.Groups;

using Events;
using RestSharp;
using Scraper.Aoseka;

public class Aoseka : BaseGroup
{
    public const string GroupKeyConstant = "aoseka";

    public override string GroupKey => GroupKeyConstant;

    public override string GroupName => "群青の世界";

    public override string CrawlCron => "0 * * * *";

    public override Task<IEnumerable<GroupEvent>> GetEvents(RestClient restClient)
    {
        var aosekaScraper = new AosekaSchedulePageScraper(restClient);
        return aosekaScraper.ScrapeAsync();
    }
}
