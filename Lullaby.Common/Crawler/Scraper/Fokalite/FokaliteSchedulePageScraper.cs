namespace Lullaby.Common.Crawler.Scraper.Fokalite;

using AngleSharp;
using Events;
using Groups;
using RestSharp;
using Ryzm;

public class FokaliteSchedulePageScraper : RyzmSchedulePageScraper, ISchedulePageScraper
{
    public FokaliteSchedulePageScraper(RestClient client, IBrowsingContext browsingContext,
        IEventTypeDetector eventTypeDetector) : base(client, browsingContext, eventTypeDetector)
    {
    }

    public override string SchedulePageUrl => "https://fokalite.com/live";
    public Type TargetGroup => typeof(Fokalite);
}
