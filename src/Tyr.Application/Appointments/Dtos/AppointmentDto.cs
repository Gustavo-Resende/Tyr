namespace Tyr.Application.Appointments.Dtos
{
    public record AppointmentInputDto(DateTime StartDateTime, Guid CustomerId, Guid ServiceId, string? Notes = null);

    public record AppointmentOutputDto(
        Guid Id,
        DateTime StartDateTime,
        DateTime EndDateTime,
        string Status,
        string? CustomerName,
        string? ServiceName,
        string? Notes
    );
}
