using Tyr.Domain.Entities;
using Tyr.Domain.ProfessionalAggregate;

namespace Tyr.Domain.ServiceAggregate
{
    public class Service : EntityBase<int>, IAggregateRoot
    {
        public Service(string? name, decimal price, int durationMinutes)
        {
            Name = name;
            Price = price;
            DurationMinutes = durationMinutes;
        }

        public string? Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }

        public List<Professional> Professionals { get; set; } = new();
    }
}
