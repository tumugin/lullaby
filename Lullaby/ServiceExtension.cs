namespace Lullaby;

using System.Reflection;
using System.Text.Json.Serialization;
using Crawler;
using Db;
using Groups;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using Services.Events;

public static class ServiceExtension
{
    private static IServiceCollection AddLullabyServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddCrawlers()
            .AddGroups();
        serviceCollection.AddScoped<IGetEventsByGroupKeyService, GetEventsByGroupKeyService>();
        serviceCollection.AddScoped<IAddEventByGroupEventService, AddEventByGroupEventService>();
        serviceCollection.AddScoped<IFindDuplicateEventService, FindDuplicateEventService>();
        serviceCollection.AddScoped<IUpdateEventByGroupEventService, UpdateEventByGroupEventService>();
        serviceCollection.AddHttpClient();
        serviceCollection.AddScoped<RestClient, RestClient>(p => new RestClient(p.GetRequiredService<HttpClient>()));
        return serviceCollection;
    }

    private static void AddLullabyHangfire(this WebApplicationBuilder webApplicationBuilder)
    {
        if (webApplicationBuilder.Environment.EnvironmentName == "Testing")
        {
            return;
        }

        webApplicationBuilder.Services.AddHangfire(hangfire =>
        {
            hangfire.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(webApplicationBuilder.Configuration.GetConnectionString("RedisConnection"));
        });
        webApplicationBuilder.Services.AddHangfireServer();
    }

    public static WebApplicationBuilder AddApplicationServices(
        this WebApplicationBuilder webApplicationBuilder
    )
    {
        webApplicationBuilder.WebHost.UseSentry(o =>
        {
            o.Dsn = webApplicationBuilder.Configuration.GetSection("Sentry").GetValue<string?>("Dsn");
            o.EnableTracing = true;
            o.TracesSampleRate = webApplicationBuilder.Configuration.GetSection("Sentry")
                .GetValue<double?>("TracesSampleRate") ?? 1.0;
        });

        var dbConnectionString = webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new InvalidOperationException("DB ConnectionString must not be null.");
        webApplicationBuilder.Services.AddDbContext<LullabyContext>(options =>
            options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString))
        );

        webApplicationBuilder
            .Services
            .AddLullabyServices()
            .AddDatabaseDeveloperPageExceptionFilter();

        webApplicationBuilder.Services.AddControllersWithViews().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        webApplicationBuilder.Services.AddMvc(options =>
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
        );

        // CORS
        webApplicationBuilder.Services.AddCors(options =>
        {
            // APIはどこからでも使えて問題ないので全オープンにする
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            });
        });

        // Add HangFire
        webApplicationBuilder.AddLullabyHangfire();

        // Swagger
        webApplicationBuilder.Services.AddSwaggerGen(swagger =>
        {
            swagger.EnableAnnotations();
            swagger.IncludeXmlComments(
                Path.Combine(
                    AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
                )
            );
        });

        webApplicationBuilder.Services.AddProblemDetails();

        return webApplicationBuilder;
    }
}
