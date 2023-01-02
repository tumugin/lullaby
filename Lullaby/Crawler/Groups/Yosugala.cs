namespace Lullaby.Crawler.Groups;

using Events;
using RestSharp;
using Scraper.Yosugala;

public class Yosugala : BaseGroup
{
    public const string GroupKeyConstant = "yosugala";
    public override string GroupKey => GroupKeyConstant;

    public override string GroupName => "yosugala";

    // every hour
    public override string CrawlCron => "0 0 * ? * * *";

    public override Task<IEnumerable<GroupEvent>> GetEvents(RestClient restClient)
    {
        var yosugalaScraper = new YosugalaSchedulePageScraper { Client = restClient };
        return yosugalaScraper.ScrapeAsync();
    }
}
