namespace Lullaby.Services.Events;

using Lullaby.Crawler.Events;
using Data;
using Models;

public class AddEventByGroupEventService : IAddEventByGroupEventService
{
    private LullabyContext Context { get; }

    public AddEventByGroupEventService(LullabyContext context) => this.Context = context;

    public async Task<Event> Execute(string groupKey, GroupEvent groupEvent, CancellationToken cancellationToken)
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
            EventType = groupEvent.EventType,
            UpdatedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow
        };
        await this.Context.Events.AddAsync(draftEvent, cancellationToken);
        await this.Context.SaveChangesAsync(cancellationToken);

        return draftEvent;
    }
}
