using Tyr.Domain.Entities;

namespace Tyr.Domain.ClienteAggregate
{
    public class Cliente : EntityBase<int>, IAggregateRoot
    {
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
    }
}
