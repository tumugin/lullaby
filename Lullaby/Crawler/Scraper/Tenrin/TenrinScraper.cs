namespace Lullaby.Crawler.Scraper.Tenrin;

using Groups;
using TimeTree;

public class TenrinScraper : TimeTreeScraper, ISchedulePageScraper
{
    public Type TargetGroup => typeof(Tenrin);

    public override string TimeTreePublicCalendarId => "35632";

    public TenrinScraper(ITimeTreeApiClient timeTreeApiClient,
        ITimeTreeScheduleGroupEventConverter timeTreeScheduleGroupEventConverter
    ) : base(
        timeTreeApiClient,
        timeTreeScheduleGroupEventConverter
    )
    {
    }
}
