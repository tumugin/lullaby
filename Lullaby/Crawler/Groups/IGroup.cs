namespace Lullaby.Crawler.Groups;

using Events;

public interface IGroup
{
	public string GroupKey { get; }

	public string GroupName { get; }

	public int CrawlInterval { get; }

	public Task<Event[]> getEvents();
}
