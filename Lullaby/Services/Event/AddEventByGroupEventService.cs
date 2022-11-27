namespace Lullaby.Services.Event;

using Crawler.Events;
using Data;
using Models;

public class AddEventByGroupEventService
{
    private LullabyContext Context { get; }

    public AddEventByGroupEventService(LullabyContext context) => Context = context;

    public async Task<Event> execute(string groupKey, GroupEvent groupEvent)
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

        var draftEvent = new Event()
        {
            GroupKey = groupKey,
            EventStarts = eventStarts,
            EventEnds = eventEnds,
            EventName = groupEvent.EventName,
            EventDescription = groupEvent.EventDescription,
            EventPlace = groupEvent.EventPlace
        };
        await Context.Events.AddAsync(draftEvent);
        await Context.SaveChangesAsync();

        return draftEvent;
    }
}
