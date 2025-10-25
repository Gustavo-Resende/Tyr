using Tyr.Domain.ProfissionalAggregate;
using Tyr.DTOs;

namespace Tyr.Api.Extensions
{
    public static class ProfessionalExtension
    {
        public static ProfissionalDto ParseDTO(this Profissional professional)
        {
            var servicosDto = professional.Servicos?
                .Select(s => new ServicoDto(s.Id, s.Nome, s.Preco))
                .ToList();

            return new ProfissionalDto(
                professional.Id,
                professional.Nome,
                servicosDto ?? new List<ServicoDto>()
            );
        }

        public static IEnumerable<ProfissionalDto> ParseDTOList(this IEnumerable<Profissional> profissionais)
            => profissionais.Select(professional => professional.ParseDTO());
    }
}
