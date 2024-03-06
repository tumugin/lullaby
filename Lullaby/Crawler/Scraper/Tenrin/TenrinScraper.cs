namespace Lullaby.Crawler.Scraper.Tenrin;

using Common.Groups;
using TimeTree;

public class TenrinScraper : TimeTreeScraper, ISchedulePageScraper
{
    public Type TargetGroup => typeof(Tenrin);

    public override string TimeTreePublicCalendarId => "tenrin_schedule";

    public TenrinScraper(ITimeTreeApiClient timeTreeApiClient,
        ITimeTreeScheduleGroupEventConverter timeTreeScheduleGroupEventConverter
    ) : base(
        timeTreeApiClient,
        timeTreeScheduleGroupEventConverter
    )
    {
    }
}
