using Ardalis.Specification;
using Tyr.Domain.CustomerAggregate;

namespace Tyr.Domain.CustomerAggregate.Specifications
{
    internal class GetCustomerByIdSpec : Specification<Customer>
    {
        public GetCustomerByIdSpec(Guid id)
        {
            Query.Where(cliente => cliente.Id == id);
        }
    }
}
