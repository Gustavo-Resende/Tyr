using Microsoft.EntityFrameworkCore;
using Tyr.Domain.Entidades; 

namespace Tyr.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
    }
}
