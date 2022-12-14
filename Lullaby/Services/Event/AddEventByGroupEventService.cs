namespace Lullaby.Services.Event;

using Crawler.Events;
using Data;
using Models;

public class AddEventByGroupEventService
{
    private LullabyContext Context { get; }

    public AddEventByGroupEventService(LullabyContext context) => this.Context = context;

    public async Task<Event> Execute(string groupKey, GroupEvent groupEvent)
    {
        var eventStarts = groupEvent.EventDateTime.EventStartDateTimeOffset;
        var eventEnds = groupEvent.EventDateTime.EventEndDateTimeOffset;

        var draftEvent = new Event
        {
            GroupKey = groupKey,
            EventStarts = eventStarts,
            EventEnds = eventEnds,
            IsDateTimeDetailed = groupEvent.EventDateTime is DetailedEventDateTime,
            EventName = groupEvent.EventName,
            EventDescription = groupEvent.EventDescription,
            EventPlace = groupEvent.EventPlace,
            EventType = groupEvent.EventType
        };
        await this.Context.Events.AddAsync(draftEvent);
        await this.Context.SaveChangesAsync();

        return draftEvent;
    }
}
