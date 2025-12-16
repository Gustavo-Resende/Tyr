using Tyr.Domain.AppointmentAggregate;
using Tyr.Application.DTOs;

namespace Tyr.Application.Extensions
{
    public static class AppointmentExtension
    {
        public static AppointmentOutputDto ParseDTO(this Appointment appointment)
            => new AppointmentOutputDto(appointment.Id, appointment.StartTime, appointment.Status,
                appointment.Customer?.Name ?? string.Empty, appointment.Professional?.Name ?? string.Empty, appointment.Service?.Name ?? string.Empty);

        public static IEnumerable<AppointmentOutputDto> ParseDTOList(this List<Appointment> appointments)
            => appointments.Select(a => a.ParseDTO()).ToList();
    }
}
