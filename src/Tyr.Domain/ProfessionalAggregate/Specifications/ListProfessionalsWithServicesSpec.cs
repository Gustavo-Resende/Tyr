using Ardalis.Specification;
using Tyr.Domain.ProfessionalAggregate;

namespace Tyr.Domain.ProfessionalAggregate.Specifications
{
    public class ListProfessionalsWithServicesSpec : Specification<Professional>
    {
        public ListProfessionalsWithServicesSpec()
        {
            Query.Include(p => p.Services);
        }
    }
}
