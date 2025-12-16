using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.Entities;

namespace Tyr.Domain.CustomerAggregate
{
    public class Customer : EntityBase<Guid>, IAggregateRoot
    {
        public Customer(string name, string phone, string? email = null)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Email = email;
            CreatedAt = DateTime.UtcNow;
            Appointments = new List<Appointment>();
        }

        public string Name { get; private set; }
        public string Phone { get; private set; }
        public string? Email { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public virtual ICollection<Appointment> Appointments { get; private set; }

        public void Update(string name, string phone, string? email = null)
        {
            Name = name;
            Phone = phone;
            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
