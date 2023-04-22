namespace Lullaby.Crawler.Groups;

public class GroupKeys
{
    private readonly Aoseka aoseka;
    private readonly Kolokol kolokol;
    private readonly Yosugala yosugala;
    private readonly Oss oss;
    private readonly Tebasen tebasen;

    public GroupKeys(Aoseka aoseka, Kolokol kolokol, Yosugala yosugala, Oss oss, Tebasen tebasen)
    {
        this.aoseka = aoseka;
        this.kolokol = kolokol;
        this.yosugala = yosugala;
        this.oss = oss;
        this.tebasen = tebasen;
    }

    public BaseGroup? GetGroupByKey(string groupKey) =>
        groupKey switch
        {
            { } s when s == Aoseka.GroupKeyConstant => this.aoseka,
            { } s when s == Kolokol.GroupKeyConstant => this.kolokol,
            { } s when s == Yosugala.GroupKeyConstant => this.yosugala,
            { } s when s == Oss.GroupKeyConstant => this.oss,
            { } s when s == Tebasen.GroupKeyConstant => this.tebasen,
            _ => null
        };
}
