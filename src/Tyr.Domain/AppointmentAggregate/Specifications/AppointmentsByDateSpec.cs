using Ardalis.Specification;
using Tyr.Domain.AppointmentAggregate;

namespace Tyr.Domain.AppointmentAggregate.Specifications
{
    public class AppointmentsByDateSpec : Specification<Appointment>
    {
        public AppointmentsByDateSpec(DateTime start, DateTime end)
        {
            Query.Where(a => a.StartDateTime >= start && a.StartDateTime < end)
                 .Include(a => a.Customer)
                 .Include(a => a.Service);
        }
    }
}
