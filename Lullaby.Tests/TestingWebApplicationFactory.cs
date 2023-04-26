namespace Lullaby.Tests;

using Lullaby.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class TestingWebApplicationFactory : WebApplicationFactory<Program>
{
    private string EnvironmentName { get; }

    public TestingWebApplicationFactory(string environmentName) => this.EnvironmentName = environmentName;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            var existingDbContextDescriptor = services
                .SingleOrDefault(v => v.ServiceType == typeof(DbContextOptions<LullabyContext>));
            if (existingDbContextDescriptor != null)
            {
                services.Remove(existingDbContextDescriptor);
            }

            services.AddDbContext<LullabyContext>(options =>
                options.UseInMemoryDatabase(TestingConstant.InMemoryTestingDatabaseName)
            );
        });

        builder.UseEnvironment(this.EnvironmentName);
    }
}
