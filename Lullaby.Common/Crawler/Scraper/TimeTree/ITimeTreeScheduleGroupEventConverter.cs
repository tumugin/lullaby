namespace Lullaby.Common.Crawler.Scraper.TimeTree;

using Events;

public interface ITimeTreeScheduleGroupEventConverter
{
    public GroupEvent Convert(TimeTreeApiResult.TimeTreeSchedule timeTreeSchedule);
}
