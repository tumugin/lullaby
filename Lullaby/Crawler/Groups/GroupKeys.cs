namespace Lullaby.Crawler.Groups;

public class GroupKeys
{
    public const string Aoseka = "aoseka";

    public static readonly string[] AvailableGroupKeys = { Aoseka };

    public static IGroup GetGroupByKey(string groupKey)
    {
        return groupKey switch
        {
            "aoseka" => new Aoseka(),
            _ => throw new ArgumentException($"Group with key {groupKey} not found")
        };
    }
}
