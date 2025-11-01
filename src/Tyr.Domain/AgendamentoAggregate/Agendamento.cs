using Tyr.Domain.ClienteAggregate;
using Tyr.Domain.Entities;
using Tyr.Domain.ProfissionalAggregate;
using Tyr.Domain.ServicoAggregate;

namespace Tyr.Domain.AgendamentoAggregate
{
    public class Agendamento : EntityBase<int>, IAggregateRoot
    {
        public DateTimeOffset? Horario { get; set; }
        public TimeSpan? Duracao { get; set; } = TimeSpan.FromMinutes(30); // Duração de 30 minutos.
        public string? Status { get; set; } = "Agendado";

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int ProfissionalId { get; set; }
        public Profissional? Profissional { get; set; }
        
        public int ServicoId { get; set; }
        public Servico? Servico { get; set; }

    }
}