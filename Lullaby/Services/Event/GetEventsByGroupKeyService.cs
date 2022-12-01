namespace Lullaby.Services.Event;

using Crawler.Events;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class GetEventsByGroupKeyService
{
    private LullabyContext LullabyContext { get; }

    public GetEventsByGroupKeyService(LullabyContext context) => this.LullabyContext = context;

    public async Task<IEnumerable<Event>> Execute(
        string groupKey,
        EventType[] eventTypes,
        DateTimeOffset startDateTimeStartRange,
        DateTimeOffset startDateTimeEndRange
    )
    {
        var result = await this.LullabyContext.Events.Where(e =>
                e.GroupKey == groupKey &&
                eventTypes.Contains(e.EventType) &&
                e.EventStarts >= startDateTimeStartRange &&
                e.EventStarts <= startDateTimeEndRange
            )
            .ToListAsync();
        return result;
    }

    public async Task<IEnumerable<Event>> Execute(string groupKey, EventType[] eventTypes)
    {
        var result = await this.LullabyContext.Events.Where(e =>
                e.GroupKey == groupKey &&
                eventTypes.Contains(e.EventType)
            )
            .ToListAsync();
        return result;
    }
}
