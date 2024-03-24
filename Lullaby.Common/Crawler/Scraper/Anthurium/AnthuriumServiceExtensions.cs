namespace Lullaby.Common.Crawler.Scraper.Anthurium;

using ApiClient;
using Microsoft.Extensions.DependencyInjection;

public static class AnthuriumServiceExtensions
{
    public static IServiceCollection AddAnthurium(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddScoped<ISchedulePageScraper, AnthuriumSchedulePageScraper>()
            .AddScoped<IAnthuriumApiClient>(v =>
                new AnthuriumApiClient(v.GetRequiredService<HttpClient>(), "https://api.anthurium-web.com/")
            );
}
