namespace Lullaby.Tests;

using Data;
using Microsoft.EntityFrameworkCore;

public class BaseDatabaseTest
{
    protected const string ConnectionString = "server=127.0.0.1;port=3306;user=root;password=root;Database=lullaby_test";

    public required LullabyContext Context { get; init; }

    public BaseDatabaseTest() =>
        this.Context = new LullabyContext(
            DatabaseConfig.CreateDbContextOptions(ConnectionString, new DbContextOptionsBuilder<LullabyContext>())
                .Options
        );

    [SetUp]
    public void PrepareDatabase()
    {
        this.Context.ChangeTracker.Clear();
        this.Context.Database.EnsureDeleted();
        this.Context.Database.EnsureCreated();
    }
}
