namespace Lullaby.Admin;

using Hangfire;

public static class WebApplicationExtension
{
    public static WebApplication UseWebApplication(this WebApplication webApplication)
    {
        // Configure the HTTP request pipeline.
        webApplication.UseStatusCodePages();

        webApplication.UseForwardedHeaders();

        if (!webApplication.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            webApplication.UseHsts();
            webApplication.UseExceptionHandler();
        }
        else
        {
            webApplication.UseDeveloperExceptionPage();
            webApplication.UseStatusCodePages();
        }

        webApplication.UseStatusCodePagesWithReExecute("/Error/{0}");

        webApplication.UseHttpsRedirection();
        webApplication.UseStaticFiles();

        webApplication.UseRouting();

        // Auth
        webApplication.UseAuthentication();
        webApplication.UseAuthorization();

        // Sentry
        webApplication.UseSentryTracing();

        webApplication.MapControllerRoute(
            "default",
            "{controller=Index}/{action=Index}/{id?}"
        );

        // HangFire
        webApplication.UseHangfireDashboard(options: new DashboardOptions
        {
            Authorization = new[] { new HangfireAuthorizationFilter() }
        });

        return webApplication;
    }
}
