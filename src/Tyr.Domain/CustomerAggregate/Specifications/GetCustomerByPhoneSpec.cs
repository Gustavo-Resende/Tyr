using Ardalis.Specification;
using Tyr.Domain.CustomerAggregate;

namespace Tyr.Domain.CustomerAggregate.Specifications
{
    public class GetCustomerByPhoneSpec : Specification<Customer>
    {
        public GetCustomerByPhoneSpec(string phone)
        {
            Query.Where(c => c.Phone == phone);
        }
    }
}
