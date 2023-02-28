namespace Lullaby.Services.Events;

using Crawler.Events;
using Models;

public interface IGetEventsByGroupKeyService
{
    public Task<IList<Event>> Execute(string groupKey,
        IEnumerable<EventType> eventTypes,
        DateTimeOffset startDateTimeStartRange,
        DateTimeOffset startDateTimeEndRange, CancellationToken cancellationToken);

    public Task<IList<Event>> Execute(string groupKey, IEnumerable<EventType> eventTypes,
        CancellationToken cancellationToken);
}
