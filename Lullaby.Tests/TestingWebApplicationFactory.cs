namespace Lullaby.Tests;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

public class TestingWebApplicationFactory : WebApplicationFactory<Program>
{
    private string EnvironmentName { get; }
    public TestingWebApplicationFactory(string environmentName)
    {
        this.EnvironmentName = environmentName;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseEnvironment(this.EnvironmentName);
    }
}
