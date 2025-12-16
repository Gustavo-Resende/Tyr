using Ardalis.Result;
using MediatR;
using Tyr.Application.Customers.Dtos;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.Customers.Extensions;

namespace Tyr.Application.Customers.Queries
{
    public record GetCustomerByIdQuery(Guid Id) : IRequest<Result<CustomerOutputDto>>;

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<CustomerOutputDto>>
    {
        private readonly IReadRepository<Customer> _repository;

        public GetCustomerByIdQueryHandler(IReadRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task<Result<CustomerOutputDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (customer is null) return Result<CustomerOutputDto>.Error("Customer not found.");
            return Result.Success(customer.ToDto());
        }
    }
}
