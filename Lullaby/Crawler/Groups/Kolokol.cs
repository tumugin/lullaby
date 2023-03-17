namespace Lullaby.Crawler.Groups;

using Events;
using RestSharp;
using Scraper.Kolokol;

public class Kolokol : BaseGroup
{
    public const string GroupKeyConstant = "kolokol";

    public override string GroupKey => GroupKeyConstant;

    public override string GroupName => "Kolokol";

    // every hour
    public override string CrawlCron => "0 0 * ? * * *";

    protected override Task<IReadOnlyList<GroupEvent>> GetEvents(RestClient restClient,
        CancellationToken cancellationToken)
    {
        var kolokolScraper = new KolokolSchedulePageScraper { Client = restClient };
        return kolokolScraper.ScrapeAsync(cancellationToken);
    }
}
