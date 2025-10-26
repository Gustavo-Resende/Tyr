using Ardalis.Specification;

namespace Tyr.Domain.AgendamentoAggregate.Specifications;

public class AgendamentosComDetalhesSpec : Specification<Agendamento>
{
    public AgendamentosComDetalhesSpec()
    {
        Query.Include(a => a.Cliente)
             .Include(a => a.Profissional)
             .Include(a => a.Servico);
    }
}