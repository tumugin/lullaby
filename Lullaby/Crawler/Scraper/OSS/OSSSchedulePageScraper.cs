namespace Lullaby.Crawler.Scraper.OSS;

using AngleSharp;
using Events;
using Groups;
using RestSharp;
using Ryzm;

public class OssSchedulePageScraper : RyzmSchedulePageScraper, ISchedulePageScraper
{
    public const string SchedulePageUrlConstant = "https://onthetreatsuperseason.com/live";

    public OssSchedulePageScraper(
        RestClient client,
        IBrowsingContext browsingContext,
        IEventTypeDetector eventTypeDetector
    ) : base(client, browsingContext, eventTypeDetector)
    {
    }

    public override string SchedulePageUrl => SchedulePageUrlConstant;
    public Type TargetGroup => typeof(Oss);
}
