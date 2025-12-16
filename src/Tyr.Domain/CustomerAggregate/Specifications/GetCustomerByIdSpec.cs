using Ardalis.Specification;
using Tyr.Domain.CustomerAggregate;

namespace Tyr.Domain.CustomerAggregate.Specifications
{
    internal class GetCustomerByIdSpec : Specification<Customer>
    {
        public GetCustomerByIdSpec(int id)
        {
            Query.Where(customer => customer.Id == id);
        }
    }
}
