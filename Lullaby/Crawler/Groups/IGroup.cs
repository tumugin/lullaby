namespace Lullaby.Crawler.Groups;

using Events;

public interface IGroup
{
    public static abstract string GroupKey { get; }

    public static abstract string GroupName { get; }

    public static abstract int CrawlInterval { get; }

    public Task<IEnumerable<GroupEvent>> getEvents();
}
