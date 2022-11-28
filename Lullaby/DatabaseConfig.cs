namespace Lullaby;

using Microsoft.EntityFrameworkCore;

public static class DatabaseConfig
{
    public static DbContextOptionsBuilder CreateDbContextOptions(
        string connectionString,
        DbContextOptionsBuilder builder
    ) =>
        builder
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
}
