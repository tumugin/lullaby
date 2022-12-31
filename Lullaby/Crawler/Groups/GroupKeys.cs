namespace Lullaby.Crawler.Groups;

public class GroupKeys
{
    public static readonly string[] AvailableGroupKeys = { Aoseka.GroupKeyConstant, Kolokol.GroupKeyConstant };

    public static BaseGroup GetGroupByKey(string groupKey) =>
        groupKey switch
        {
            { } s when s == Aoseka.GroupKeyConstant => new Aoseka(),
            { } s when s == Kolokol.GroupKeyConstant => new Kolokol(),
            _ => throw new ArgumentException($"Group with key {groupKey} not found")
        };
}
