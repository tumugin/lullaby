namespace Lullaby.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class LullabyContext : DbContext
{
    public DbSet<Schedule> Schedules { get; set; }
}
