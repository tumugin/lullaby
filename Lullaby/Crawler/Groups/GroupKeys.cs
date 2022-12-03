namespace Lullaby.Crawler.Groups;

public class GroupKeys
{
    public static readonly string[] AvailableGroupKeys = { Aoseka.GroupKey };

    public static IGroup GetGroupByKey(string groupKey)
    {
        return groupKey switch
        {
            "aoseka" => new Aoseka(),
            _ => throw new ArgumentException($"Group with key {groupKey} not found")
        };
    }
}
