using Ardalis.Result;
using MediatR;
using Tyr.Application.Customers.Dtos;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.Customers.Extensions;

namespace Tyr.Application.Customers.Queries
{
    public record GetAllCustomersQuery : IRequest<Result<IReadOnlyList<CustomerOutputDto>>>;

    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, Result<IReadOnlyList<CustomerOutputDto>>>
    {
        private readonly IReadRepository<Customer> _repository;

        public GetAllCustomersQueryHandler(IReadRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task<Result<IReadOnlyList<CustomerOutputDto>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _repository.ListAsync(cancellationToken);
            var dtos = customers.Select(c => c.ToDto()).ToList();
            return Result.Success<IReadOnlyList<CustomerOutputDto>>(dtos.AsReadOnly());
        }
    }
}
