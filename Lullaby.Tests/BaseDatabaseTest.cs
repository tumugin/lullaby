namespace Lullaby.Tests;

using Lullaby.Db;
using Microsoft.EntityFrameworkCore;

public class BaseDatabaseTest
{
    protected LullabyContext Context { get; private set; } = null!;

    [SetUp]
    public void Initialize() =>
        this.Context = new LullabyContext(
            new DbContextOptionsBuilder<LullabyContext>()
                .UseInMemoryDatabase(TestingConstant.InMemoryTestingDatabaseName)
                .Options
        );

    [TearDown]
    public void Cleanup()
    {
        this.Context.Database.EnsureDeleted();
        this.Context.Dispose();
    }
}
