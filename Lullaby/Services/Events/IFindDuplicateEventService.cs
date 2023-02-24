namespace Lullaby.Services.Events;

using Models;

public interface IFindDuplicateEventService
{
    public Task<List<Event>> Execute(
        IEnumerable<EventSearchQueryData> eventSearchQueryData
    );

    public class EventSearchQueryData
    {
        public required string GroupKey { get; init; }
        public required string EventName { get; init; }
        public required DateTimeOffset StartDateTime { get; init; }
        public required DateTimeOffset EndDateTime { get; init; }
    }
}
