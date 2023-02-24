namespace Lullaby.Crawler.Groups;

using Events;
using RestSharp;
using Scraper.OSS;

public class OSS : BaseGroup
{
    public const string GroupKeyConstant = "oss";
    public override string GroupKey => GroupKeyConstant;
    public override string GroupName => "On the treat Super Season";
    public override string CrawlCron => "0 0 * ? * * *";

    protected override Task<IEnumerable<GroupEvent>> GetEvents(
        RestClient restClient,
        CancellationToken cancellationToken
    )
    {
        var ossScraper = new OSSSchedulePageScraper { Client = restClient };
        return ossScraper.ScrapeAsync(cancellationToken);
    }
}
