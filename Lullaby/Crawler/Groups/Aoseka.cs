namespace Lullaby.Crawler.Groups;

using Events;

public class Aoseka : IGroup
{
    public string GroupKey => "aoseka";

    public string GroupName => "群青の世界";

    public int CrawlInterval => 60 * 60;

    public Task<GroupEvent[]> getEvents() => throw new NotImplementedException();
}
