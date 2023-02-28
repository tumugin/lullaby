namespace Lullaby.Services.Events;

using Lullaby.Crawler.Events;
using Lullaby.Data;
using Lullaby.Models;
using Microsoft.EntityFrameworkCore;

public class GetEventsByGroupKeyService : IGetEventsByGroupKeyService
{
    private LullabyContext LullabyContext { get; }

    public GetEventsByGroupKeyService(LullabyContext context) => this.LullabyContext = context;

    public async Task<IList<Event>> Execute(string groupKey,
        IEnumerable<EventType> eventTypes,
        DateTimeOffset startDateTimeStartRange,
        DateTimeOffset startDateTimeEndRange,
        CancellationToken cancellationToken)
    {
        var result = await this.LullabyContext.Events.Where(e =>
                e.GroupKey == groupKey &&
                eventTypes.Contains(e.EventType) &&
                e.EventStarts >= startDateTimeStartRange &&
                e.EventStarts <= startDateTimeEndRange
            )
            .ToListAsync(cancellationToken);
        return result;
    }

    public async Task<IList<Event>> Execute(string groupKey,
        IEnumerable<EventType> eventTypes,
        CancellationToken cancellationToken)
    {
        var result = await this.LullabyContext.Events.Where(e =>
                e.GroupKey == groupKey &&
                eventTypes.Contains(e.EventType)
            )
            .ToListAsync(cancellationToken);
        return result;
    }
}
