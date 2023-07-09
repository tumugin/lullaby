namespace Lullaby;

using Hangfire;
using Job;

public static class WebApplicationExtension
{
    private static void UseLullabyHangfire(this WebApplication webApplication)
    {
        if (webApplication.Environment.EnvironmentName == "Testing")
        {
            return;
        }

        ConfigureScheduledJobs.Configure(webApplication.Services.GetRequiredService<IRecurringJobManager>());
        if (webApplication.Environment.IsDevelopment())
        {
            // TODO: Implement HangFire dashboard authentication for production
            webApplication.UseHangfireDashboard();
        }
    }

    public static WebApplication UseLullabyWebApplication(this WebApplication webApplication)
    {
        // Configure the HTTP request pipeline.
        webApplication.UseStatusCodePages();

        if (!webApplication.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            webApplication.UseHsts();
            webApplication.UseExceptionHandler();
        }
        else
        {
            webApplication.UseDeveloperExceptionPage();
        }

        // HangFire
        webApplication.UseLullabyHangfire();

        // Swagger(enable for all envs because it's api application open for everyone)
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI();

        webApplication.UseHttpsRedirection();
        webApplication.UseStaticFiles();

        webApplication.UseRouting();

        // Sentry
        webApplication.UseSentryTracing();

        webApplication.UseCors();

        webApplication.UseAuthorization();

        webApplication.MapControllerRoute(
            "default",
            "{controller=Index}/{action=Index}/{id?}"
        );

        return webApplication;
    }
}
