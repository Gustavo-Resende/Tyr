using Tyr.Domain.Entities;
using Tyr.Domain.ProfissionalAggregate;

namespace Tyr.Domain.ServicoAggregate
{
    public class Servico : EntityBase<int>, IAggregateRoot
    {
        public string? Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }

        public int ProfissionalId { get; set; }
        public Profissional? Profissional { get; set; }
    }
}