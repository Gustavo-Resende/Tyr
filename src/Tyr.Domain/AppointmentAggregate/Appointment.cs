using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.Entities;
using Tyr.Domain.ProfessionalAggregate;
using Tyr.Domain.ServiceAggregate;

namespace Tyr.Domain.AppointmentAggregate
{
    public class Appointment : EntityBase<int>, IAggregateRoot
    {
        public DateTimeOffset? StartTime { get; set; }
        public TimeSpan? Duration { get; set; } = TimeSpan.FromMinutes(30);
        public string? Status { get; set; } = "Scheduled";

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int ProfessionalId { get; set; }
        public Professional? Professional { get; set; }
        
        public int ServiceId { get; set; }
        public Service? Service { get; set; }

    }
}
