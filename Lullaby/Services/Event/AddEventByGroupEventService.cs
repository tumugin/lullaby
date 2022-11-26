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
        var eventStarts = groupEvent.EventDateTime is DetailedEventDateTime
            ? ((DetailedEventDateTime)groupEvent.EventDateTime).EventStartDateTime
            : groupEvent.EventDateTime is UnDetailedEventDateTime
                ? ((UnDetailedEventDateTime)groupEvent.EventDateTime).EventStartDate
                : throw new ArgumentException("EventDateTime is not a valid type");
        var eventEnds = groupEvent.EventDateTime is DetailedEventDateTime
            ? ((DetailedEventDateTime)groupEvent.EventDateTime).EventEndDateTime
            : groupEvent.EventDateTime is UnDetailedEventDateTime
                ? ((UnDetailedEventDateTime)groupEvent.EventDateTime).EventEndDate
                : throw new ArgumentException("EventDateTime is not a valid type");

        var draftEvent = new Event()
        {
            GroupKey = groupKey,
            EventStarts = eventStarts,
            EventEnds = eventEnds,
            EventName = groupEvent.EventName,
            EventDescription = groupEvent.EventDescription,
            EventPlace = groupEvent.EventPlace
        };

        return (await Context.Events.AddAsync(draftEvent)).Entity;
    }
}
