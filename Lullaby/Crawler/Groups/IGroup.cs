namespace Lullaby.Crawler.Groups;

using Events;

public interface IGroup
{
    public abstract string GroupKey { get; }

    public abstract string GroupName { get; }

    public abstract int CrawlInterval { get; }

    public Task<IEnumerable<GroupEvent>> getEvents();
}
