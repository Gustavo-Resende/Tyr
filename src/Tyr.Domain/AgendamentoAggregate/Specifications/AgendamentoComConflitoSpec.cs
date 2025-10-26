using Ardalis.Specification;
using Tyr.Domain.AgendamentoAggregate;

namespace Tyr.Domain.AgendamentoAggregate.Specifications
{
    public class AgendamentoComConflitoSpec : Specification<Agendamento>
    {
        public AgendamentoComConflitoSpec(int profissionalId, DateTimeOffset novoHorarioInicio, TimeSpan duracaoDoServico)
        {
            DateTimeOffset novoHorarioFim = novoHorarioInicio.Add(duracaoDoServico);

            Query.Where(agendamentoExistente =>
                agendamentoExistente.ProfissionalId == profissionalId &&

                agendamentoExistente.Horario.HasValue &&
                agendamentoExistente.Duracao.HasValue &&

                novoHorarioInicio < (agendamentoExistente.Horario.Value.Add(agendamentoExistente.Duracao.Value)) &&
                novoHorarioFim > agendamentoExistente.Horario.Value
            );

            Query.AsNoTracking();
        }
    }
}