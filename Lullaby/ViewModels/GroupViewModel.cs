namespace Lullaby.ViewModels;

using Crawler.Groups;

public class GroupViewModel
{
    public required string GroupKey { get; init; }

    public required string GroupName { get; init; }

    public required string CrawlCron { get; init; }

    public static GroupViewModel FromGroup(BaseGroup baseGroup)
    {
        return new GroupViewModel
        {
            GroupKey = baseGroup.GroupKey, GroupName = baseGroup.GroupName, CrawlCron = baseGroup.CrawlCron
        };
    }
}
