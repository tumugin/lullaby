namespace Lullaby.Crawler.Scraper.TimeTree;

public static class TimeTreeServiceExtensions
{
    public static IServiceCollection AddTimeTree(this IServiceCollection serviceCollection) =>
        serviceCollection.AddScoped<ITimeTreeApiClient, TimeTreeApiClient>();
}