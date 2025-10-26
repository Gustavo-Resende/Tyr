using Tyr.Domain.AgendamentoAggregate;
using Tyr.DTOs;

namespace Tyr.Api.Extensions
{
    public static class AgendamentoExtension
    {
        public static AgendamentoOutputDto ParseDTO(this Agendamento agendamento)
            => new AgendamentoOutputDto(agendamento.Id, agendamento.Horario, agendamento.Status,
                agendamento.Cliente?.Nome ?? string.Empty, agendamento.Profissional?.Nome ?? string.Empty, agendamento.Servico?.Nome ?? string.Empty);

        public static IEnumerable<AgendamentoOutputDto> ParseDTOList(this List<Agendamento> agendamentos)
            => agendamentos.Select(a => a.ParseDTO()).ToList();
    }
}
