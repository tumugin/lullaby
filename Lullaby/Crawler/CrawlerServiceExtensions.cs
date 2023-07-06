namespace Lullaby.Crawler;

using AngleSharp;
using Events;
using Scraper;
using Scraper.Aoseka;
using Scraper.Axelight;
using Scraper.Kolokol;
using Scraper.OSS;
using Scraper.Prsmin;
using Scraper.Tebasen;
using Scraper.Tenhana;
using Scraper.TimeTree;
using Scraper.Yosugala;

public static class CrawlerServiceExtensions
{
    public static IServiceCollection AddCrawlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBrowsingContext>(_ =>
            BrowsingContext.New(Configuration.Default.WithDefaultLoader())
        );
        serviceCollection.AddScoped<IEventTypeDetector, EventTypeDetector>();

        serviceCollection.AddTimeTree();

        serviceCollection
            .AddScoped<ISchedulePageScraper, AosekaSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, KolokolSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, OssSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, YosugalaSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, TebasenSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, AxelightSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, PrsminSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, TenhanaScraper>();

        serviceCollection.AddScoped<IGroupCrawler, GroupCrawler>();

        return serviceCollection;
    }
}
