namespace Lullaby.ViewModels;

using Crawler.Events;
using Models;

public class EventViewModel
{
    public required long Id { get; init; }

    public required string GroupKey { get; init; }

    public required DateTimeOffset EventStarts { get; init; }

    public required DateTimeOffset EventEnds { get; init; }

    public required bool IsDateTimeDetailed { get; init; }

    public required string EventName { get; init; }

    public required string EventDescription { get; init; }

    public required string? EventPlace { get; init; }

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
            EventType = eventModel.EventType,
        };
}
