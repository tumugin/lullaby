namespace Lullaby.Crawler.Groups;

using Events;
using Services.Event;

public abstract class BaseGroup
{
    public abstract string GroupKey { get; }

    public abstract string GroupName { get; }

    public abstract string CrawlCron { get; }

    public abstract Task<IEnumerable<GroupEvent>> getEvents();

    public async Task getAndUpdateSavedEvents(
        AddEventByGroupEventService addEventByGroupEventService,
        FindDuplicateEventService findDuplicateEventService
    )
    {
        var events = await this.getEvents();
    }
}
