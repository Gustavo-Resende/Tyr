using System;
using Tyr.Domain.Entities;

namespace Tyr.Domain.AppointmentAggregate
{
    public class Appointment : EntityBase<Guid>, IAggregateRoot
    {
        public Appointment(Guid customerId, Guid serviceId, DateTime startDateTime, string? notes = null)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            ServiceId = serviceId;
            StartDateTime = startDateTime;
            Notes = notes;
            Status = AppointmentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid CustomerId { get; private set; }
        public Guid ServiceId { get; private set; }
        public DateTime StartDateTime { get; private set; }
        public DateTime EndDateTime { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string? Notes { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public virtual Tyr.Domain.CustomerAggregate.Customer? Customer { get; private set; }
        public virtual Tyr.Domain.ServiceAggregate.Service? Service { get; private set; }

        // keep helper for compatibility if needed
        public void CalculateEndDateTime(int durationMinutes)
        {
            EndDateTime = StartDateTime.AddMinutes(durationMinutes);
        }

        public void UpdateStatus(AppointmentStatus status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStart(DateTime newStart)
        {
            StartDateTime = newStart;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
