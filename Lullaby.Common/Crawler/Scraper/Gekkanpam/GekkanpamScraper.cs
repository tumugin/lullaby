namespace Lullaby.Common.Crawler.Scraper.Gekkanpam;

using Groups;
using TimeTree;

public class GekkanpamScraper : TimeTreeScraper, ISchedulePageScraper
{
    public GekkanpamScraper(
        ITimeTreeApiClient timeTreeApiClient,
        ITimeTreeScheduleGroupEventConverter timeTreeScheduleGroupEventConverter
    ) : base(timeTreeApiClient, timeTreeScheduleGroupEventConverter)
    {
    }

    public override string TimeTreePublicCalendarId => "gekkanpam";
    public Type TargetGroup => typeof(Gekkanpam);
}
