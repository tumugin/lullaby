namespace Lullaby.Crawler.Groups;

using Events;
using RestSharp;
using Scraper.Aoseka;

public class Aoseka : IGroup
{
    public static string GroupKey => "aoseka";

    public static string GroupName => "群青の世界";

    public static int CrawlInterval => 60 * 60;

    public Task<IEnumerable<GroupEvent>> getEvents()
    {
        var aosekaScraper = new AosekaSchedulePageScraper(new RestClient());
        return aosekaScraper.ScrapeAsync();
    }
}
