namespace Lullaby.Crawler.Scraper.Yosugala;

using AngleSharp;
using Events;
using Groups;
using RestSharp;
using Ryzm;

public class YosugalaSchedulePageScraper : RyzmSchedulePageScraper, ISchedulePageScraper
{
    public const string SchedulePageUrlConstant = "https://yosugala2022.ryzm.jp/live";

    public YosugalaSchedulePageScraper(
        RestClient client,
        IBrowsingContext browsingContext,
        IEventTypeDetector eventTypeDetector
    ) : base(client, browsingContext, eventTypeDetector)
    {
    }

    public override string SchedulePageUrl => SchedulePageUrlConstant;
    public Type TargetGroup => typeof(Yosugala);
}
