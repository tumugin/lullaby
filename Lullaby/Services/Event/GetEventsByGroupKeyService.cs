namespace Lullaby.Services.Event;

using Crawler.Events;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class GetEventsByGroupKeyService
{
    private LullabyContext LullabyContext { get; }

    public GetEventsByGroupKeyService(LullabyContext lullabyContext) => this.LullabyContext = lullabyContext;

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
