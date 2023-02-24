namespace Lullaby;

public static class WebApplicationExtension
{
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
            name: "default",
            pattern: "{controller=Index}/{action=Index}/{id?}"
        );

        return webApplication;
    }
}
