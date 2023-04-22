namespace Lullaby.Crawler.Groups;

using Events;

public class Tebasen : BaseGroup
{
    public const string GroupKeyConstant = "tebasen";
    public const string CrawlCronConstant = "0 0 * ? * * *";

    public override string GroupKey => GroupKeyConstant;
    public override string GroupName => "手羽先センセーション";
    public override string CrawlCron => CrawlCronConstant;
    protected override Task<IReadOnlyList<GroupEvent>> GetEvents(CancellationToken cancellationToken) => throw new NotImplementedException();
}
