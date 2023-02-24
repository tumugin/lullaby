namespace Lullaby.Services.Events;

using Crawler.Events;
using Models;

public interface IAddEventByGroupEventService
{
    public Task<Event> Execute(string groupKey, GroupEvent groupEvent);
}
