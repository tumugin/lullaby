namespace Lullaby.Crawler.Scraper.Tenhana;

using Groups;
using TimeTree;

public class TenhanaScraper : TimeTreeScraper, ISchedulePageScraper
{
    public override string TimeTreePublicCalendarId => "54197";

    public Type TargetGroup => typeof(Tenhana);

    public TenhanaScraper(
        ITimeTreeApiClient timeTreeApiClient,
        ITimeTreeScheduleGroupEventConverter timeTreeScheduleGroupEventConverter
    ) : base(timeTreeApiClient,
        timeTreeScheduleGroupEventConverter)
    {
    }
}
