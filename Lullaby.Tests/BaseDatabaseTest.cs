namespace Lullaby.Tests;

using Database.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class BaseDatabaseTest
{
    protected LullabyContext Context { get; private set; } = null!;

    protected static DbContextOptions BuildDbContextOptions(
        DbContextOptionsBuilder builder
    ) =>
        builder
            .UseInMemoryDatabase(TestingConstant.InMemoryTestingDatabaseName)
            .ConfigureWarnings(c => c.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

    [SetUp]
    public void Initialize() =>
        this.Context = new LullabyContext(
            BuildDbContextOptions(new DbContextOptionsBuilder<LullabyContext>())
        );

    [TearDown]
    public void Cleanup()
    {
        this.Context.Database.EnsureDeleted();
        this.Context.Dispose();
    }
}
