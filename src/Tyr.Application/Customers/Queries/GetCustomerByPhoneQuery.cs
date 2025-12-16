using Ardalis.Result;
using MediatR;
using Tyr.Application.Customers.Dtos;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Domain.CustomerAggregate.Specifications;
using System.Linq;
using Tyr.Application.Customers.Extensions;

namespace Tyr.Application.Customers.Queries
{
    public record GetCustomerByPhoneQuery(string Phone) : IRequest<Result<CustomerOutputDto>>;

    public class GetCustomerByPhoneQueryHandler : IRequestHandler<GetCustomerByPhoneQuery, Result<CustomerOutputDto>>
    {
        private readonly IReadRepository<Customer> _repository;

        public GetCustomerByPhoneQueryHandler(IReadRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task<Result<CustomerOutputDto>> Handle(GetCustomerByPhoneQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetCustomerByPhoneSpec(request.Phone);
            var customer = (await _repository.ListAsync(spec, cancellationToken)).FirstOrDefault();
            if (customer is null) return Result<CustomerOutputDto>.Error("Customer not found.");
            return Result<CustomerOutputDto>.Success(customer.ToDto());
        }
    }
}
