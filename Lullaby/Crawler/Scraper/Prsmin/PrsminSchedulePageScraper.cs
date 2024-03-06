namespace Lullaby.Crawler.Scraper.Prsmin;

using AngleSharp;
using Common.Groups;
using Events;
using RestSharp;
using Ryzm;

public class PrsminSchedulePageScraper : RyzmSchedulePageScraper, ISchedulePageScraper
{
    public override string SchedulePageUrl => "https://prsmin.com/live";
    public Type TargetGroup => typeof(Prsmin);

    public PrsminSchedulePageScraper(
        RestClient client,
        IBrowsingContext browsingContext,
        IEventTypeDetector eventTypeDetector
    ) : base(client, browsingContext, eventTypeDetector)
    {
    }
}
