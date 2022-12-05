namespace Lullaby.Crawler.Groups;

using Events;
using RestSharp;
using Scraper.Aoseka;

public class Aoseka : IGroup
{
    public const string GroupKeyConstant = "aoseka";

    public string GroupKey => GroupKeyConstant;

    public string GroupName => "群青の世界";

    public string CrawlCron => "0 * * * *";

    public Task<IEnumerable<GroupEvent>> getEvents()
    {
        var aosekaScraper = new AosekaSchedulePageScraper(new RestClient());
        return aosekaScraper.ScrapeAsync();
    }
}
