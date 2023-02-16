namespace Lullaby.Tests;

using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class BaseDatabaseTest
{
    protected LullabyContext Context { get; }

    public BaseDatabaseTest()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Testing" });
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("DefaultConnection should not be null.");
        this.Context = new LullabyContext(
            DatabaseConfig.CreateDbContextOptions(connectionString, new DbContextOptionsBuilder<LullabyContext>())
                .Options
        );
    }

    [SetUp]
    public void PrepareDatabase()
    {
        this.Context.ChangeTracker.Clear();
        this.Context.Database.EnsureDeleted();
        this.Context.Database.EnsureCreated();
    }
}
