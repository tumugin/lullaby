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
        var eventStarts = groupEvent.EventDateTime switch
        {
            DetailedEventDateTime detailedEventDateTime => detailedEventDateTime.EventStartDateTime,
            UnDetailedEventDateTime unDetailedEventDateTime => unDetailedEventDateTime.EventStartDate,
            _ => throw new ArgumentException("EventDateTime is not a valid type"),
        };
        var eventEnds = groupEvent.EventDateTime switch
        {
            DetailedEventDateTime detailedEventDateTime => detailedEventDateTime.EventEndDateTime,
            UnDetailedEventDateTime unDetailedEventDateTime => unDetailedEventDateTime.EventEndDate,
            _ => throw new ArgumentException("EventDateTime is not a valid type"),
        };

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
