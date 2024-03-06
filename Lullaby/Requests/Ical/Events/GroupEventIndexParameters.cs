namespace Lullaby.Requests.Ical.Events;

using Common.Enums;
using Crawler.Events;

public class GroupEventIndexParameters
{
    /// <summary>
    ///     Start date and time of events to be searched
    /// </summary>
    public DateTimeOffset? EventStartsFrom { get; set; }

    /// <summary>
    ///     End date and time of events to be searched
    /// </summary>
    public DateTimeOffset? EventEndsAt { get; set; }

    /// <summary>
    ///     Event types to be searched
    /// </summary>
    public IReadOnlyCollection<EventType> EventTypes { get; set; } = EventTypeExt.AllTypes();
}
