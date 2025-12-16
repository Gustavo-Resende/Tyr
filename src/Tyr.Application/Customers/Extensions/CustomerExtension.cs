using Tyr.Domain.CustomerAggregate;
using Tyr.Application.Customers.Dtos;

namespace Tyr.Application.Customers.Extensions
{
    public static class CustomerExtension
    {
        public static CustomerOutputDto ToDto(this Customer customer)
            => new(customer.Id, customer.Name, customer.Phone, customer.Email);

        public static IEnumerable<CustomerOutputDto> ToDtoList(this IEnumerable<Customer> customers)
            => customers.Select(c => c.ToDto());
    }
}
