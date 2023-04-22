namespace Lullaby.Crawler.Scraper.Yosugala;

using AngleSharp;
using Events;
using RestSharp;
using Ryzm;

public class YosugalaSchedulePageScraper : RyzmSchedulePageScraper
{
    public const string SchedulePageUrlConstant = "https://yosugala2022.ryzm.jp/live";

    public override string SchedulePageUrl => SchedulePageUrlConstant;

    public YosugalaSchedulePageScraper(
        RestClient client,
        IBrowsingContext browsingContext,
        IEventTypeDetector eventTypeDetector
    ) : base(client, browsingContext, eventTypeDetector)
    {
    }
}
