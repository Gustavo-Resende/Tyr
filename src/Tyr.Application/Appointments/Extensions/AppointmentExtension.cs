using Tyr.Domain.AppointmentAggregate;
using Tyr.Application.Appointments.Dtos;

namespace Tyr.Application.Appointments.Extensions
{
    public static class AppointmentExtension
    {
        public static AppointmentOutputDto ToDto(this Appointment a)
            => new(a.Id, a.StartDateTime, a.EndDateTime, a.Status.ToString(), a.Customer?.Name, a.Service?.Name, a.Notes);

        public static IEnumerable<AppointmentOutputDto> ToDtoList(this IEnumerable<Appointment> apps)
            => apps.Select(a => a.ToDto()).ToList();
    }
}
