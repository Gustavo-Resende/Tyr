using Ardalis.Specification;
using Tyr.Domain.AppointmentAggregate;

namespace Tyr.Domain.AppointmentAggregate.Specifications
{
    public class AppointmentConflictSpec : Specification<Appointment>
    {
        public AppointmentConflictSpec(Guid serviceId, DateTime start, DateTime end)
        {
            // check any appointment for the same service that overlaps the given window
            Query.Where(a => a.ServiceId == serviceId &&
                             a.StartDateTime < end &&
                             a.EndDateTime > start);
        }
    }
}
