namespace Lullaby.Crawler.Scraper.OSS;

using Ryzm;

public class OSSSchedulePageScraper : RyzmSchedulePageScraper
{
    public const string SchedulePageUrlConstant = "https://onthetreatsuperseason.com/live";
    public override string SchedulePageUrl => SchedulePageUrlConstant;
}
