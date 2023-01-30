namespace Lullaby.Crawler.Groups;

using Events;
using RestSharp;

public class OSS : BaseGroup
{
    public const string GroupKeyConstant = "oss";
    public override string GroupKey => GroupKeyConstant;
    public override string GroupName => "On the treat Super Season";
    public override string CrawlCron => "0 0 * ? * * *";

    public override Task<IEnumerable<GroupEvent>> GetEvents(RestClient restClient) =>
        throw new NotImplementedException();
}
