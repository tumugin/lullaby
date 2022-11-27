namespace Lullaby;

using Microsoft.EntityFrameworkCore;

public static class DatabaseConfig
{
    public static DbContextOptionsBuilder createDbContextOptions(
        string connectionString,
        DbContextOptionsBuilder builder
    ) =>
        builder
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
}
