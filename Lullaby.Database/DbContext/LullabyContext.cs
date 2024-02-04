namespace Lullaby.Database.DbContext;

using Microsoft.EntityFrameworkCore;
using Models;

public class LullabyContext : DbContext
{
    public LullabyContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; } = null!;
}
