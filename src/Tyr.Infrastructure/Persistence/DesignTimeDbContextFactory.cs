using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tyr.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Try environment variable, fallback to a sensible default for local development
            var connectionString = Environment.GetEnvironmentVariable("DefaultConnection")
                                   ?? "Host=localhost;Database=tyr;Username=postgres;Password=postgres";

            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
