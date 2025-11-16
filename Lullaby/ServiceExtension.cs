namespace Lullaby;

using System.Reflection;
using System.Text.Json.Serialization;
using Common.Groups;
using Database.DbContext;
using Db;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Jobs;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.Events;

public static class ServiceExtension
{
    private static void AddLullabyServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddGroups();
        serviceCollection.TryAddSingleton(_ => TimeProvider.System);
        serviceCollection.AddScoped<IGetEventsByGroupKeyService, GetEventsByGroupKeyService>();
        serviceCollection.AddHttpClient();

        // TODO: Job実行を分離させる
        serviceCollection.AddLullabyJobsServices();
    }

    private static void AddLullabyHangfire(this WebApplicationBuilder webApplicationBuilder)
    {
        if (webApplicationBuilder.IsTestingEnvironment())
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

    private static bool IsTestingEnvironment(this WebApplicationBuilder webApplicationBuilder)
        => webApplicationBuilder.Environment.EnvironmentName == "Testing";

    public static WebApplicationBuilder AddApplicationServices(
        this WebApplicationBuilder webApplicationBuilder
    )
    {
        webApplicationBuilder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            webApplicationBuilder.Configuration.GetSection("ForwardedHeaders")
                .GetSection("KnownNetworks")
                .Get<string[]>()?
                .Select(System.Net.IPNetwork.Parse)
                .ToList()
                .ForEach(x => options.KnownIPNetworks.Add(x));
            options.ForwardLimit = null;
        });

        webApplicationBuilder.WebHost.UseSentry(o =>
        {
            o.Dsn = webApplicationBuilder.Configuration.GetSection("Sentry").GetValue<string?>("Dsn");
            o.TracesSampleRate = webApplicationBuilder.Configuration.GetSection("Sentry")
                .GetValue<double?>("TracesSampleRate") ?? 1.0;
        });

        var dbConnectionString = webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new InvalidOperationException("DB ConnectionString must not be null.");

        // If it is on testing env, don't register DBContext as
        // "Only a single database provider can be registered in a service provider"
        // error cause since from .NET 9.0 version NpgSQL libs.
        if (!webApplicationBuilder.IsTestingEnvironment())
        {
            webApplicationBuilder.Services.AddDbContext<LullabyContext>(options =>
                options.UseNpgsql(dbConnectionString)
            );
        }

        webApplicationBuilder
            .Services
            .AddLullabyServices();

        if (webApplicationBuilder.Environment.EnvironmentName == "Development")
        {
            webApplicationBuilder.Services.AddDatabaseDeveloperPageExceptionFilter();
        }

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

        // OpenAPI
        webApplicationBuilder.Services.AddOpenApi();

        webApplicationBuilder.Services.AddProblemDetails();

        return webApplicationBuilder;
    }
}
