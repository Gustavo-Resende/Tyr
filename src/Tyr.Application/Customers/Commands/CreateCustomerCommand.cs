using Ardalis.Result;
using MediatR;
using Tyr.Application.Customers.Dtos;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.Customers.Extensions;

namespace Tyr.Application.Customers.Commands
{
    public record CreateCustomerCommand(string Name, string Phone, string? Email) : IRequest<Result<CustomerOutputDto>>;

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerOutputDto>>
    {
        private readonly IRepository<Customer> _repository;

        public CreateCustomerCommandHandler(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task<Result<CustomerOutputDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) return Result<CustomerOutputDto>.Error("Name is required.");
            if (string.IsNullOrWhiteSpace(request.Phone)) return Result<CustomerOutputDto>.Error("Phone is required.");

            var customer = new Customer(request.Name, request.Phone, request.Email);

            await _repository.AddAsync(customer, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success(customer.ToDto());
        }
    }
}
