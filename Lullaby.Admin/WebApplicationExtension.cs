namespace Lullaby.Admin;

using Hangfire;

public static class WebApplicationExtension
{
    public static WebApplication UseWebApplication(this WebApplication webApplication)
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
            webApplication.UseStatusCodePages();
        }

        webApplication.UseStatusCodePagesWithReExecute("/Error/{0}");

        // HangFire
        webApplication.UseHangfireDashboard();

        webApplication.UseHttpsRedirection();
        webApplication.UseStaticFiles();

        webApplication.UseRouting();

        // Sentry
        webApplication.UseSentryTracing();

        webApplication.UseAuthentication();
        webApplication.UseAuthorization();

        webApplication.MapControllerRoute(
            "default",
            "{controller=Index}/{action=Index}/{id?}"
        );

        return webApplication;
    }
}
