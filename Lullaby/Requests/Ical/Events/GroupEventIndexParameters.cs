namespace Lullaby.Requests.Ical.Events;

using Crawler.Events;

public class GroupEventIndexParameters
{
    public DateTimeOffset? EventStartsFrom { get; set; }
    public DateTimeOffset? EventEndsAt { get; set; }
    public EventType[] EventTypes { get; set; } = EventTypeExt.AllTypes();
}
