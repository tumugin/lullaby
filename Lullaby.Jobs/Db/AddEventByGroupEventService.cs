namespace Lullaby.Jobs.Db;

using Common.Crawler.Events;
using Database.DbContext;
using Database.Models;
using Services.Crawler.Events;

public class AddEventByGroupEventService : IAddEventByGroupEventService
{
    public AddEventByGroupEventService(LullabyContext context) => this.Context = context;
    private LullabyContext Context { get; }

    public async Task<Event> Execute(string groupKey, GroupEvent groupEvent, CancellationToken cancellationToken)
    {
        var eventStarts = groupEvent.EventDateTime.EventStartDateTimeOffset;
        var eventEnds = groupEvent.EventDateTime.EventEndDateTimeOffset;

        var draftEvent = new Event
        {
            GroupKey = groupKey,
            EventStarts = eventStarts.ToUniversalTime(),
            EventEnds = eventEnds.ToUniversalTime(),
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
