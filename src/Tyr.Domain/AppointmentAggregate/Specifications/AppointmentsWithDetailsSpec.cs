using Ardalis.Specification;
using Tyr.Domain.AppointmentAggregate;

namespace Tyr.Domain.AppointmentAggregate.Specifications
{
    public class AppointmentsWithDetailsSpec : Specification<Appointment>
    {
        public AppointmentsWithDetailsSpec()
        {
            Query.Include(a => a.Customer)
                 .Include(a => a.Professional)
                 .Include(a => a.Service);
        }
    }
}
