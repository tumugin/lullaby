namespace Lullaby.Crawler.Scraper.OSS;

using RestSharp;
using Ryzm;

public class OSSSchedulePageScraper : RyzmSchedulePageScraper
{
    public const string SchedulePageUrlConstant = "https://onthetreatsuperseason.com/live";
    public override string SchedulePageUrl => SchedulePageUrlConstant;

    public OSSSchedulePageScraper(RestClient client) : base(client)
    {
    }
}
