namespace Lullaby.Admin;

using System.Text.Json.Serialization;
using Database.DbContext;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class ServiceExtension
{
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
            options.UseNpgsql(dbConnectionString)
        );

        webApplicationBuilder.Services.AddControllersWithViews()
            .AddRazorRuntimeCompilation()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        webApplicationBuilder.Services.AddMvc(options =>
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
        );

        webApplicationBuilder.Services.AddRazorPages();

        webApplicationBuilder.Services.AddProblemDetails();

        webApplicationBuilder.AddLullabyHangfire();

        return webApplicationBuilder;
    }
}
