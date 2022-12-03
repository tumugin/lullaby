namespace Lullaby.Crawler.Groups;

public class GroupKeys
{
    public static readonly string[] AvailableGroupKeys = { Aoseka.GroupKeyConstant };

    public static IGroup GetGroupByKey(string groupKey) =>
        groupKey switch
        {
            { } s when s == Aoseka.GroupKeyConstant => new Aoseka(),
            _ => throw new ArgumentException($"Group with key {groupKey} not found")
        };
}
