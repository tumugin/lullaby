namespace Lullaby.Crawler;

using Groups;
using Scraper.Aoseka;
using Scraper.Kolokol;
using Scraper.OSS;
using Scraper.Yosugala;

public static class CrawlerServiceExtensions
{
    public static IServiceCollection AddCrawlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<Aoseka, Aoseka>();
        serviceCollection.AddScoped<AosekaSchedulePageScraper, AosekaSchedulePageScraper>();

        serviceCollection.AddScoped<Kolokol, Kolokol>();
        serviceCollection.AddScoped<KolokolSchedulePageScraper, KolokolSchedulePageScraper>();

        serviceCollection.AddScoped<OSS, OSS>();
        serviceCollection.AddScoped<OSSSchedulePageScraper, OSSSchedulePageScraper>();

        serviceCollection.AddScoped<Yosugala, Yosugala>();
        serviceCollection.AddScoped<YosugalaSchedulePageScraper, YosugalaSchedulePageScraper>();

        serviceCollection.AddScoped<GroupKeys, GroupKeys>();

        return serviceCollection;
    }
}
