using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.Entities;

namespace Tyr.Domain.ServiceAggregate
{
    public class Service : EntityBase<Guid>, IAggregateRoot
    {
        public Service(string name, decimal price, int durationInMinutes, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DurationInMinutes = durationInMinutes > 0 ? durationInMinutes : throw new ArgumentException("Duration must be > 0", nameof(durationInMinutes));
            Price = price > 0 ? price : throw new ArgumentException("Price must be > 0", nameof(price));
            Description = description;
            CreatedAt = DateTime.UtcNow;
            Appointments = new List<Appointment>();
        }

        public string Name { get; set; }
        public string? Description { get; set; }
        public int DurationInMinutes { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }

        public void Update(string name, int durationInMinutes, decimal price, string? description = null)
        {
            Name = name;
            DurationInMinutes = durationInMinutes;
            Price = price;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
