namespace Lullaby.Tests;

using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

        // Cleanup the in-memory database
        var context = new LullabyContext(
            new DbContextOptionsBuilder<LullabyContext>()
                .UseInMemoryDatabase(TestingConstant.InMemoryTestingDatabaseName)
                .Options
        );
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        builder.UseEnvironment(this.EnvironmentName);
    }
}
