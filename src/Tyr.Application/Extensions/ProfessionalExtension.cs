using Tyr.Application.DTOs;
using Tyr.Application.Services.Dtos;
using Tyr.Domain.ProfissionalAggregate;

namespace Tyr.Api.Extensions
{
    public static class ProfessionalExtension
    {
        public static ProfissionalDto ParseDTO(this Profissional professional)
        {
            var servicosDto = professional.Servicos?
                .Select(s => new ServicoOutputDto(s.Id, s.Nome, s.Preco, professional.Id))
                .ToList();

            return new ProfissionalDto(
                professional.Id,
                professional.Nome,
                servicosDto ?? new List<ServicoOutputDto>()
            );
        }

        public static IEnumerable<ProfissionalDto> ParseDTOList(this IEnumerable<Profissional> profissionais)
            => profissionais.Select(professional => professional.ParseDTO());
    }
}
