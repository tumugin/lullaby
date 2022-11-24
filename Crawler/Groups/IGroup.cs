namespace lullaby.Crawler.Groups;

using lullaby.Crawler.Events;

public interface IGroup
{
	public string GroupKey { get; }

	public string GroupName { get; }

	public int CrawlInterval { get; }

	public Task<Event[]> getEvents();
}
