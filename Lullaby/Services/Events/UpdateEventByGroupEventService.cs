namespace Lullaby.Services.Events;

using Lullaby.Crawler.Events;
using Lullaby.Data;
using Lullaby.Models;
using Lullaby.Utils;

public class UpdateEventByGroupEventService
{
    private LullabyContext Context { get; }

    public UpdateEventByGroupEventService(LullabyContext context) => this.Context = context;

    public async Task<Event> Execute(Event eventEntity, GroupEvent groupEvent)
    {
        var eventStarts = groupEvent.EventDateTime.EventStartDateTimeOffset;
        var eventEnds = groupEvent.EventDateTime.EventEndDateTimeOffset;

        eventEntity.Also(entity =>
        {
            entity.EventStarts = eventStarts;
            entity.EventEnds = eventEnds;
            entity.IsDateTimeDetailed = groupEvent.EventDateTime is DetailedEventDateTime;
            entity.EventName = groupEvent.EventName;
            entity.EventDescription = groupEvent.EventDescription;
            entity.EventPlace = groupEvent.EventPlace;
            entity.EventType = groupEvent.EventType;
        });

        var updatedEntity = this.Context.Events.Update(eventEntity);
        await this.Context.SaveChangesAsync();

        return updatedEntity.Entity;
    }
}
