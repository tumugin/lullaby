namespace Lullaby.Crawler.Groups;

using Events;
using RestSharp;
using Scraper.Aoseka;

public class Aoseka : IGroup
{
    public string GroupKey => "aoseka";

    public string GroupName => "群青の世界";

    public int CrawlInterval => 60 * 60;

    public Task<IEnumerable<GroupEvent>> getEvents()
    {
        var aosekaScraper = new AosekaSchedulePageScraper(new RestClient());
        return aosekaScraper.ScrapeAsync();
    }
}
