namespace Lullaby.Db;

using Crawler.Events;
using Models;
using Services.Events;
using Utils;

public class UpdateEventByGroupEventService : IUpdateEventByGroupEventService
{
    public UpdateEventByGroupEventService(LullabyContext context) => this.Context = context;
    private LullabyContext Context { get; }

    public async Task<Event> Execute(Event eventEntity, GroupEvent groupEvent, CancellationToken cancellationToken)
    {
        var eventStarts = groupEvent.EventDateTime.EventStartDateTimeOffset;
        var eventEnds = groupEvent.EventDateTime.EventEndDateTimeOffset;

        eventEntity.Also(entity =>
        {
            entity.EventStarts = eventStarts.ToUniversalTime();
            entity.EventEnds = eventEnds.ToUniversalTime();
            entity.IsDateTimeDetailed = groupEvent.EventDateTime is DetailedEventDateTime;
            entity.EventName = groupEvent.EventName;
            entity.EventDescription = groupEvent.EventDescription;
            entity.EventPlace = groupEvent.EventPlace;
            entity.EventType = groupEvent.EventType;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
        });

        var updatedEntity = this.Context.Events.Update(eventEntity);
        await this.Context.SaveChangesAsync(cancellationToken);

        return updatedEntity.Entity;
    }
}
