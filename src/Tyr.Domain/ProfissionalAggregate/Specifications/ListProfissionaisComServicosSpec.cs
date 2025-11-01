using Ardalis.Specification;

namespace Tyr.Domain.ProfissionalAggregate.Specifications
{
    public class ListProfissionaisComServicosSpec : Specification<Profissional>
    {
        public ListProfissionaisComServicosSpec()
        {
            Query.Include(p => p.Servicos);
        }
    }
}
