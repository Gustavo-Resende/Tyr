using Ardalis.Specification;
using Tyr.Domain.AppointmentAggregate;

namespace Tyr.Domain.AppointmentAggregate.Specifications
{
    public class AppointmentConflictSpec : Specification<Appointment>
    {
        public AppointmentConflictSpec(int professionalId, DateTimeOffset newStart, TimeSpan serviceDuration)
        {
            DateTimeOffset newEnd = newStart.Add(serviceDuration);

            Query.Where(existing =>
                existing.ProfessionalId == professionalId &&

                existing.StartTime.HasValue &&
                existing.Duration.HasValue &&

                newStart < (existing.StartTime.Value.Add(existing.Duration.Value)) &&
                newEnd > existing.StartTime.Value
            );

            Query.AsNoTracking();
        }
    }
}
