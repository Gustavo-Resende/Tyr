using Ardalis.Result;
using MediatR;
using Tyr.Application.Customers.Dtos;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.Customers.Extensions;
using Tyr.Domain.CustomerAggregate.Specifications;

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

            var phoneNormalized = request.Phone.Trim();
            var emailNormalized = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email!.Trim().ToLowerInvariant();

            var spec = new GetCustomerByPhoneOrEmailSpec(phoneNormalized, emailNormalized);
            var existing = (await _repository.ListAsync(spec, cancellationToken)).FirstOrDefault();
            if (existing != null)
            {
                return Result<CustomerOutputDto>.Error("Customer already exists.");
            }

            var customer = new Customer(request.Name.Trim(), phoneNormalized, emailNormalized);

            await _repository.AddAsync(customer, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success(customer.ToDto());
        }
    }
}
