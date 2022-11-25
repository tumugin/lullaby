namespace Lullaby.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class LullabyContext : DbContext
{
    public LullabyContext(DbContextOptions options) : base(options) {
    }

    public DbSet<Schedule> Schedules { get; set; }
}
