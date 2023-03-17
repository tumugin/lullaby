namespace Lullaby.Tests;

using Data;
using Microsoft.EntityFrameworkCore;

public class BaseDatabaseTest
{
    protected LullabyContext Context { get; }

    public BaseDatabaseTest() =>
        this.Context = new LullabyContext(
            new DbContextOptionsBuilder<LullabyContext>()
                .UseInMemoryDatabase(nameof(BaseDatabaseTest))
                .Options
        );

    [TearDown]
    public void Cleanup()
    {
        this.Context.Database.EnsureDeleted();
        this.Context.Dispose();
    }
}
