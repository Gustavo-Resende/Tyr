using Tyr.Application.DTOs;
using Tyr.Domain.CustomerAggregate;

namespace Tyr.Application.Extensions
{
    public static class CustomerExtension
    {
        public static CustomerOutputDto ParseDTO(this Customer customer)
            => new(customer.Id, customer.Name, customer.Phone);

        public static IEnumerable<CustomerOutputDto> ParseDTOList(this IEnumerable<Customer> customers)
            => customers.Select(customer => customer.ParseDTO());
    }
}
