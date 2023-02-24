namespace Lullaby.Services.Events;

using Crawler.Events;
using Models;

public interface IUpdateEventByGroupEventService
{
    public Task<Event> Execute(Event eventEntity, GroupEvent groupEvent);
}
