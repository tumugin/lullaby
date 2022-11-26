namespace Lullaby.Tests;

using Data;
using Microsoft.EntityFrameworkCore;

public class BaseDatabaseTest
{
    private const string ConnectionString = "server=127.0.0.1;port=3306;user=root;password=root;Database=lullaby_test";

    public required LullabyContext Context { get; init; }

    public BaseDatabaseTest()
    {
        Context = new LullabyContext(
            new DbContextOptionsBuilder<LullabyContext>()
                .UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString))
                .Options
        );
    }

    [SetUp] public void PrepareDatabase()
    {
        Context.ChangeTracker.Clear();
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }
}
