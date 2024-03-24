namespace Lullaby.Jobs.Services.Crawler.Events;

using Common.Groups;

public interface ICleanupOldScheduledEventsService
{
    public Task ExecuteAsync(IGroup targetGroup, CancellationToken cancellationToken);
}
