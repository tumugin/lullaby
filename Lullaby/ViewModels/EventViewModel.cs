namespace Lullaby.ViewModels;

using Crawler.Events;
using Models;

public class EventViewModel
{
    public long Id { get; }

    public string GroupKey { get; }

    public DateTimeOffset EventStarts { get; }

    public DateTimeOffset EventEnds { get; }

    public bool IsDateTimeDetailed { get; }

    public string EventName { get; }

    public string EventDescription { get; }

    public string? EventPlace { get; }

    public EventType EventType { get; }

    public EventViewModel(Event eventModel)
    {
        this.Id = eventModel.Id;
        this.GroupKey = eventModel.GroupKey;
        this.EventStarts = eventModel.EventStarts;
        this.EventEnds = eventModel.EventEnds;
        this.IsDateTimeDetailed = eventModel.IsDateTimeDetailed;
        this.EventName = eventModel.EventName;
        this.EventDescription = eventModel.EventDescription;
        this.EventPlace = eventModel.EventPlace;
        this.EventType = eventModel.EventType;
    }
}
