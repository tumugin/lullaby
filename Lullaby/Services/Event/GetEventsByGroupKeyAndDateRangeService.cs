namespace Lullaby.Services.Event;

using Crawler.Events;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class GetEventsByGroupKeyAndDateRangeService
{
    private LullabyContext LullabyContext { get; }

    public GetEventsByGroupKeyAndDateRangeService(LullabyContext context) => this.LullabyContext = context;

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
}
