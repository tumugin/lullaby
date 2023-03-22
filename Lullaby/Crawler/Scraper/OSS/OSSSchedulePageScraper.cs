namespace Lullaby.Crawler.Scraper.OSS;

using RestSharp;
using Ryzm;

public class OssSchedulePageScraper : RyzmSchedulePageScraper
{
    public const string SchedulePageUrlConstant = "https://onthetreatsuperseason.com/live";
    public override string SchedulePageUrl => SchedulePageUrlConstant;

    public OssSchedulePageScraper(RestClient client) : base(client)
    {
    }
}
