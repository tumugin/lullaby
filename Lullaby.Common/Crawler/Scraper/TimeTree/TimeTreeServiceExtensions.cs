namespace Lullaby.Common.Crawler.Scraper.TimeTree;

using Microsoft.Extensions.DependencyInjection;

public static class TimeTreeServiceExtensions
{
    public static IServiceCollection AddTimeTree(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddScoped<ITimeTreeScheduleGroupEventConverter, TimeTreeScheduleGroupEventConverter>()
            .AddScoped<ITimeTreeApiClient, TimeTreeApiClient>();
}
