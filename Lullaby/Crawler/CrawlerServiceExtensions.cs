namespace Lullaby.Crawler;

using AngleSharp;
using AngleSharp.Html.Parser;
using Events;
using Scraper;
using Scraper.Anthurium;
using Scraper.Aoseka;
using Scraper.Axelight;
using Scraper.Kolokol;
using Scraper.OSS;
using Scraper.Prsmin;
using Scraper.Tebasen;
using Scraper.Tenhana;
using Scraper.Tenrin;
using Scraper.TimeTree;
using Scraper.Yosugala;
using Utility;

public static class CrawlerServiceExtensions
{
    public static IServiceCollection AddCrawlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBrowsingContext>(_ =>
            BrowsingContext.New(Configuration.Default.WithDefaultLoader())
        );
        serviceCollection.AddScoped<IHtmlParser, HtmlParser>();
        serviceCollection.AddScoped<IEventTypeDetector, EventTypeDetector>();

        serviceCollection.AddTimeTree();

        serviceCollection.AddScoped<IFullDateGuesser, FullDateGuesser>();

        serviceCollection
            .AddScoped<ISchedulePageScraper, AosekaSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, KolokolSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, OssSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, YosugalaSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, TebasenSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, AxelightSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, PrsminSchedulePageScraper>()
            .AddScoped<ISchedulePageScraper, TenhanaScraper>()
            .AddScoped<ISchedulePageScraper, TenrinScraper>();

        serviceCollection.AddAnthurium();

        serviceCollection.AddScoped<IGroupCrawler, GroupCrawler>();

        return serviceCollection;
    }
}
