namespace Lullaby.Jobs;

using Common.Crawler;
using Common.Groups;
using Db;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using Services.Crawler;
using Services.Crawler.Events;

public static class LullabyJobsServiceExtension
{
    public static IServiceCollection AddLullabyJobsServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient();
        serviceCollection.AddScoped<RestClient, RestClient>(p => new RestClient(p.GetRequiredService<HttpClient>()));
        serviceCollection.AddGroups();
        serviceCollection.AddCrawlers();
        serviceCollection.AddScoped<IAddEventByGroupEventService, AddEventByGroupEventService>();
        serviceCollection.AddScoped<IFindDuplicateEventService, FindDuplicateEventService>();
        serviceCollection.AddScoped<IUpdateEventByGroupEventService, UpdateEventByGroupEventService>();
        serviceCollection.AddScoped<IGroupCrawlerService, GroupCrawlerServiceService>();

        return serviceCollection;
    }
}
