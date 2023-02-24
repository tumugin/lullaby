namespace Lullaby.Services.Events;

using Crawler.Events;
using Models;

public interface IGetEventsByGroupKeyService
{
    public Task<IEnumerable<Event>> Execute(string groupKey,
        EventType[] eventTypes,
        DateTimeOffset startDateTimeStartRange,
        DateTimeOffset startDateTimeEndRange, CancellationToken cancellationToken);

    public Task<IEnumerable<Event>> Execute(string groupKey, EventType[] eventTypes,
        CancellationToken cancellationToken);
}
