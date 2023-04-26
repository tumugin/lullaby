namespace Lullaby.Crawler;

using Groups;

public interface IGroupCrawler
{
    public Task GetAndUpdateSavedEvents(
        IGroup group,
        CancellationToken cancellationToken
    );
}
