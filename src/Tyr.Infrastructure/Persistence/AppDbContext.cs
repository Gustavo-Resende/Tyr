using Microsoft.EntityFrameworkCore;
using Tyr.Domain.ServiceAggregate;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.BusinessHourAggregate;

namespace Tyr.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<BusinessHour> BusinessHours { get; set; }
    }
}
