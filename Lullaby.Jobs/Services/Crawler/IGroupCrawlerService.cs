namespace Lullaby.Jobs.Services.Crawler;

using Common.Groups;

public interface IGroupCrawlerService
{
    public Task GetAndUpdateSavedEvents(
        IGroup group,
        CancellationToken cancellationToken
    );
}
