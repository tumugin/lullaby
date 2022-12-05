namespace Lullaby.Crawler.Groups;

using Events;

public interface IGroup
{
    public string GroupKey { get; }

    public string GroupName { get; }

    public string CrawlCron { get; }

    public Task<IEnumerable<GroupEvent>> getEvents();
}
