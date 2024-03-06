namespace Lullaby.ViewModels;

using Common.Enums;
using Crawler.Events;
using Database.Models;

public class EventViewModel
{
    /// <summary>
    ///     Event ID
    /// </summary>
    public required long Id { get; init; }

    /// <summary>
    ///     Group Key
    /// </summary>
    public required string GroupKey { get; init; }

    /// <summary>
    ///     Start time and date of the event
    /// </summary>
    public required DateTimeOffset EventStarts { get; init; }

    /// <summary>
    ///     Enc time and date of the event
    /// </summary>
    public required DateTimeOffset EventEnds { get; init; }

    /// <summary>
    ///     Whether the event time is detailed(If not, it is only the date and time will be 00:00:00 at groups timezone)
    /// </summary>
    public required bool IsDateTimeDetailed { get; init; }

    /// <summary>
    ///     Name of the event
    /// </summary>
    public required string EventName { get; init; }

    /// <summary>
    ///     Description of the event
    /// </summary>
    public required string EventDescription { get; init; }

    /// <summary>
    ///     Place of the event
    /// </summary>
    public required string? EventPlace { get; init; }

    /// <summary>
    ///     Event type of the event
    /// </summary>
    public required EventType EventType { get; init; }

    public static EventViewModel FromEvent(Event eventModel) =>
        new()
        {
            Id = eventModel.Id,
            GroupKey = eventModel.GroupKey,
            EventStarts = eventModel.EventStarts,
            EventEnds = eventModel.EventEnds,
            IsDateTimeDetailed = eventModel.IsDateTimeDetailed,
            EventName = eventModel.EventName,
            EventDescription = eventModel.EventDescription,
            EventPlace = eventModel.EventPlace,
            EventType = eventModel.EventType
        };
}
