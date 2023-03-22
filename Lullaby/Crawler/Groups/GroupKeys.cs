namespace Lullaby.Crawler.Groups;

public class GroupKeys
{
    private readonly Aoseka aoseka;
    private readonly Kolokol kolokol;
    private readonly Yosugala yosugala;
    private readonly Oss oss;

    public GroupKeys(Aoseka aoseka, Kolokol kolokol, Yosugala yosugala, Oss oss)
    {
        this.aoseka = aoseka;
        this.kolokol = kolokol;
        this.yosugala = yosugala;
        this.oss = oss;
    }

    public BaseGroup? GetGroupByKey(string groupKey) =>
        groupKey switch
        {
            { } s when s == Aoseka.GroupKeyConstant => this.aoseka,
            { } s when s == Kolokol.GroupKeyConstant => this.kolokol,
            { } s when s == Yosugala.GroupKeyConstant => this.yosugala,
            { } s when s == Oss.GroupKeyConstant => this.oss,
            _ => null
        };
}
