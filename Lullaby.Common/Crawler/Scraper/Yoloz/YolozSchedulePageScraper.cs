namespace Lullaby.Common.Crawler.Scraper.Yoloz;

using AngleSharp;
using Events;
using Groups;
using RestSharp;
using Ryzm;

public class YolozSchedulePageScraper : RyzmSchedulePageScraper, ISchedulePageScraper
{
    public YolozSchedulePageScraper(
        RestClient client,
        IBrowsingContext browsingContext,
        IEventTypeDetector eventTypeDetector
    ) : base(client, browsingContext, eventTypeDetector)
    {
    }

    public override string SchedulePageUrl => "https://yoloz.tokyo/live";
    public Type TargetGroup => typeof(Yoloz);
}
