namespace Lullaby.Admin.Db;

using Database.DbContext;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Services;
using Utils;

public class EventSearchService(LullabyContext dbContext) : IEventSearchService
{
    public async Task<SearchEventResult> SearchEventAsync(string? groupKey,
        string? eventName,
        DateTimeOffset? startDateTime,
        DateTimeOffset? endDateTime,
        int page,
        CancellationToken cancellationToken)
    {
        IQueryable<Event> events = dbContext.Events;

        if (groupKey != null)
        {
            events = events.Where(e => e.GroupKey == groupKey);
        }

        if (eventName != null)
        {
            events = events.Where(e => e.EventName.Contains(eventName));
        }

        if (startDateTime != null)
        {
            events = events.Where(e => e.EventStarts >= startDateTime);
        }

        if (endDateTime != null)
        {
            events = events.Where(e => e.EventEnds <= endDateTime);
        }

        var results = await events
            .OrderBy(v => v.EventStarts)
            .Paginate(50, page)
            .ToArrayAsync(cancellationToken);

        var totalEvents = await events.CountAsync(cancellationToken);

        return new SearchEventResult
        {
            Events = results,
            CurrentPage = page,
            Limit = 50,
            TotalEvents = totalEvents
        };
    }
}
