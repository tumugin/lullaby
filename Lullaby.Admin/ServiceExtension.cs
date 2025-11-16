namespace Lullaby.Admin;

using System.Text.Json.Serialization;
using Common.Groups;
using Database.DbContext;
using Db;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using StackExchange.Redis;

public static class ServiceExtension
{
    private static void AddLullabyServices(this IServiceCollection services)
    {
        services.AddGroups();
        services.AddScoped<IGroupStatisticsService, GroupStatisticsService>();
        services.AddScoped<IEventSearchService, EventSearchService>();
        services.AddScoped<IUserInterfaceDateTimeOffsetService, UserInterfaceDateTimeOffsetService>(_ =>
            new UserInterfaceDateTimeOffsetService(TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo"))
        );
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
    }

    private static void AddLullabyDataProtection(this WebApplicationBuilder webApplicationBuilder)
    {
        var redisConnection = ConnectionMultiplexer.Connect(
            webApplicationBuilder.Configuration.GetConnectionString("RedisConnection") ??
            throw new InvalidOperationException("RedisConnection string must not be null.")
        );
        webApplicationBuilder.Services.AddDataProtection()
            .PersistKeysToStackExchangeRedis(redisConnection);
    }

    private static void AddLullabyOidcAuthentication(this WebApplicationBuilder webApplicationBuilder)
    {
        var oidcConfigSection = webApplicationBuilder.Configuration.GetSection("Oidc");
        webApplicationBuilder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromHours(6))
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ClientId = oidcConfigSection.GetValue<string>("ClientId");
                options.ClientSecret = oidcConfigSection.GetValue<string>("ClientSecret");
                options.MetadataAddress = oidcConfigSection.GetValue<string>("MetadataAddress");
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.CallbackPath = "/oidc/callback";
                options.RemoteSignOutPath = "/oidc/remote-sign-out";
                options.SignedOutCallbackPath = "/oidc/signed-out";
                options.AccessDeniedPath = "/oidc/access-denied";
            });
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
        webApplicationBuilder.Services.AddLullabyServices();

        webApplicationBuilder.AddLullabyDataProtection();
        webApplicationBuilder.AddLullabyOidcAuthentication();

        webApplicationBuilder.Services.AddAuthorizationBuilder()
            .AddPolicy("Hangfire", policy => policy.RequireAuthenticatedUser());

        return webApplicationBuilder;
    }
}
