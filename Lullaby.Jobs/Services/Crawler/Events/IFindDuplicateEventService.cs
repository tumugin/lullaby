namespace Lullaby.Jobs.Services.Crawler.Events;

using Database.Models;

public interface IFindDuplicateEventService
{
    public Task<IReadOnlyList<Event>> Execute(IEnumerable<EventSearchQueryData> eventSearchQueryData,
        CancellationToken cancellationToken);

    public class EventSearchQueryData
    {
        public required string GroupKey { get; init; }
        public required string EventName { get; init; }
        public required DateTimeOffset StartDateTime { get; init; }
        public required DateTimeOffset EndDateTime { get; init; }
    }
}
