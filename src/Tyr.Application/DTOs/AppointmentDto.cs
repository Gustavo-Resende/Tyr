namespace Tyr.Application.DTOs
{
    public record AppointmentInputDto(DateTimeOffset? StartTime, int CustomerId, int ProfessionalId, int ServiceId);
    public record AppointmentOutputDto(
        int Id,
        DateTimeOffset? StartTime,
        string? Status,
        string? CustomerName,
        string? ProfessionalName,
        string? ServiceName
    );
}
