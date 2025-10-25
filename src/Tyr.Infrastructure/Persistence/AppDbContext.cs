using Microsoft.EntityFrameworkCore;
using Tyr.Domain.AgendamentoAggregate;
using Tyr.Domain.ClienteAggregate;
using Tyr.Domain.ProfissionalAggregate;
using Tyr.Domain.ServicoAggregate;

namespace Tyr.Infrastructure.Persistence
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
