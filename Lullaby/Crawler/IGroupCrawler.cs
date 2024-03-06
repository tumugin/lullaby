namespace Lullaby.Crawler;

using Common.Groups;

public interface IGroupCrawler
{
    public Task GetAndUpdateSavedEvents(
        IGroup group,
        CancellationToken cancellationToken
    );
}
