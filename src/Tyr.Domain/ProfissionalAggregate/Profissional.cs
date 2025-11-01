using Tyr.Domain.Entities;
using Tyr.Domain.ServicoAggregate;

namespace Tyr.Domain.ProfissionalAggregate
{
    public class Profissional : EntityBase<int>, IAggregateRoot
    {
        public string? Nome { get; set; } = string.Empty;
        public List<Servico> Servicos { get; set; } = new();
    }
}
