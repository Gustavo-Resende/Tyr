using Tyr.Application.DTOs;
using Tyr.Application.Services.Dtos;
using Tyr.Domain.ProfessionalAggregate;
using Tyr.Domain.ServiceAggregate;

namespace Tyr.Application.Extensions
{
    public static class ProfessionalExtension
    {
        public static ProfessionalDto ParseDTO(this Professional professional)
        {
            var servicesDto = professional.Services?
                .Select(s => new ServiceDto(s.Id, s.Name, s.Price, professional.Id))
                .ToList();

            return new ProfessionalDto(
                professional.Id,
                professional.Name,
                servicesDto ?? new List<ServiceDto>()
            );
        }

        public static IEnumerable<ProfessionalDto> ParseDTOList(this IEnumerable<Professional> professionals)
            => professionals.Select(professional => professional.ParseDTO());
    }
}
