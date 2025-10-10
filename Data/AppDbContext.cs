using Microsoft.EntityFrameworkCore;
using Tyr.Models;

namespace Tyr.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
    }
}
