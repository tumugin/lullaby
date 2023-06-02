namespace Lullaby.Tests;

using Lullaby.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class TestingWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string environmentName;
    private readonly Action<DbContextOptionsBuilder> dbContextOptionsBuilder;

    public TestingWebApplicationFactory(string environmentName, Action<DbContextOptionsBuilder> dbContextOptionsBuilder)
    {
        this.environmentName = environmentName;
        this.dbContextOptionsBuilder = dbContextOptionsBuilder;
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

            services.AddDbContext<LullabyContext>(this.dbContextOptionsBuilder);
        });

        builder.UseEnvironment(this.environmentName);
    }
}
