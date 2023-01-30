namespace Lullaby.Crawler.Scraper.Yosugala;

using Ryzm;

public class YosugalaSchedulePageScraper : RyzmSchedulePageScraper
{
    public const string SchedulePageUrlConstant = "https://yosugala2022.ryzm.jp/live";

    public override string SchedulePageUrl => SchedulePageUrlConstant;
}
