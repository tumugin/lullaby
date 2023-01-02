namespace Lullaby.Crawler.Groups;

public class GroupKeys
{
    public static BaseGroup? GetGroupByKey(string groupKey) =>
        groupKey switch
        {
            { } s when s == Aoseka.GroupKeyConstant => new Aoseka(),
            { } s when s == Kolokol.GroupKeyConstant => new Kolokol(),
            { } s when s == Yosugala.GroupKeyConstant => new Yosugala(),
            _ => null
        };
}
