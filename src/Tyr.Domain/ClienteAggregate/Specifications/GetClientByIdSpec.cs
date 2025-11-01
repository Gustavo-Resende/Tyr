using Ardalis.Specification;

namespace Tyr.Domain.ClienteAggregate.Specifications
{
    internal class GetClientByIdSpec : Specification<Cliente>
    {
        public GetClientByIdSpec(int id)
        {
            Query.Where(cliente => cliente.Id == id);
        }
    }
}
