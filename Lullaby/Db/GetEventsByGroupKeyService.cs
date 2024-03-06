namespace Lullaby.Db;

using Common.Enums;
using Crawler.Events;
using Database.DbContext;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Services.Events;

public class GetEventsByGroupKeyService : IGetEventsByGroupKeyService
{
    public GetEventsByGroupKeyService(LullabyContext context) => this.LullabyContext = context;
    private LullabyContext LullabyContext { get; }

    public async Task<IReadOnlyList<Event>> Execute(string groupKey,
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

    public async Task<IReadOnlyList<Event>> Execute(string groupKey,
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
