namespace Lullaby.Requests.Ical.Events;

using Crawler.Events;

public class GroupEventIndexParameters
{
    public DateTimeOffset? EventStartsFrom { get; }
    public DateTimeOffset? EventEndsAt { get; }
    public EventType[] EventTypes { get; } = EventTypeExt.AllTypes();
}
