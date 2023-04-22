namespace Lullaby.Crawler;

using AngleSharp;
using Events;
using Groups;
using Scraper.Aoseka;
using Scraper.Kolokol;
using Scraper.OSS;
using Scraper.Tebasen;
using Scraper.Yosugala;

public static class CrawlerServiceExtensions
{
    public static IServiceCollection AddCrawlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBrowsingContext>(_ =>
            BrowsingContext.New(Configuration.Default.WithDefaultLoader())
        );
        serviceCollection.AddScoped<IEventTypeDetector, EventTypeDetector>();

        serviceCollection.AddScoped<Aoseka, Aoseka>();
        serviceCollection.AddScoped<AosekaSchedulePageScraper, AosekaSchedulePageScraper>();

        serviceCollection.AddScoped<Kolokol, Kolokol>();
        serviceCollection.AddScoped<KolokolSchedulePageScraper, KolokolSchedulePageScraper>();

        serviceCollection.AddScoped<Oss, Oss>();
        serviceCollection.AddScoped<OssSchedulePageScraper, OssSchedulePageScraper>();

        serviceCollection.AddScoped<Yosugala, Yosugala>();
        serviceCollection.AddScoped<YosugalaSchedulePageScraper, YosugalaSchedulePageScraper>();

        serviceCollection.AddScoped<Tebasen, Tebasen>();
        serviceCollection.AddScoped<TebasenSchedulePageScraper, TebasenSchedulePageScraper>();

        serviceCollection.AddScoped<GroupKeys, GroupKeys>();

        return serviceCollection;
    }
}
