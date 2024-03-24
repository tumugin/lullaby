namespace Lullaby.Jobs.Services.Crawler.Events;

using Common.Crawler.Events;
using Database.Models;

public interface IUpdateEventByGroupEventService
{
    public Task<Event> Execute(Event eventEntity, GroupEvent groupEvent, CancellationToken cancellationToken);
}
