namespace Lullaby.Jobs.Services.Crawler.Events;

using Common.Crawler.Events;
using Database.Models;

public interface IAddEventByGroupEventService
{
    public Task<Event> Execute(string groupKey, GroupEvent groupEvent, CancellationToken cancellationToken);
}
