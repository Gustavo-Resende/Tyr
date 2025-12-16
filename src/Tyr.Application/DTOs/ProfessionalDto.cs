using Tyr.Application.Services.Dtos;

namespace Tyr.Application.DTOs
{
    public record ProfessionalDto(int Id, string? Name, List<ServiceDto> Services);
}
