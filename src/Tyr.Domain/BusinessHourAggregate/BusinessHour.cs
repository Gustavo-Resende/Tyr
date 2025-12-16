using System;
using Tyr.Domain.Entities;

namespace Tyr.Domain.BusinessHourAggregate
{
    public class BusinessHour : EntityBase<Guid>, IAggregateRoot
    {
        public BusinessHour(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, bool isActive = true)
        {
            Id = Guid.NewGuid();
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
            IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
        }

        public DayOfWeek DayOfWeek { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public void Update(TimeSpan start, TimeSpan end, bool isActive)
        {
            if (start >= end) throw new ArgumentException("Start must be before End");
            StartTime = start;
            EndTime = end;
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
