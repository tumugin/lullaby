namespace Lullaby.Services.Events;

using Common.Enums;
using Database.Models;

public interface IGetEventsByGroupKeyService
{
    public Task<IReadOnlyList<Event>> Execute(string groupKey,
        IEnumerable<EventType> eventTypes,
        DateTimeOffset startDateTimeStartRange,
        DateTimeOffset startDateTimeEndRange, CancellationToken cancellationToken);

    public Task<IReadOnlyList<Event>> Execute(string groupKey, IEnumerable<EventType> eventTypes,
        CancellationToken cancellationToken);
}
