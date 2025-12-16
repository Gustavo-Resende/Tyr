using Tyr.Domain.Entities;
using Tyr.Domain.ServiceAggregate;

namespace Tyr.Domain.ProfessionalAggregate
{
    public class Professional : EntityBase<int>, IAggregateRoot
    {
        public string? Name { get; set; } = string.Empty;
        public List<Service> Services { get; set; } = new();
    }
}
