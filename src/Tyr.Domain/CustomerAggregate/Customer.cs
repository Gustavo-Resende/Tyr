using Tyr.Domain.Entities;

namespace Tyr.Domain.CustomerAggregate
{
    public class Customer : EntityBase<int>, IAggregateRoot
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
    }
}
