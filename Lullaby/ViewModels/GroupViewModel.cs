namespace Lullaby.ViewModels;

using Crawler.Groups;

public class GroupViewModel
{
    public required string GroupKey { get; init; }

    public required string GroupName { get; init; }

    public required int CrawlInterval { get; init; }

    public static GroupViewModel FromGroup(IGroup group)
    {
        return new GroupViewModel
        {
            GroupKey = group.GroupKey,
            GroupName = group.GroupName,
            CrawlInterval = group.CrawlInterval
        };
    }
}
