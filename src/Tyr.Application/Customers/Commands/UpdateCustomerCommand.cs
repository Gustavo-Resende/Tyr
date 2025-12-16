using Ardalis.Result;
using MediatR;
using Tyr.Application.Customers.Dtos;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.Customers.Extensions;

namespace Tyr.Application.Customers.Commands
{
    public record UpdateCustomerCommand(Guid Id, string Name, string Phone, string? Email) : IRequest<Result<CustomerOutputDto>>;

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<CustomerOutputDto>>
    {
        private readonly IRepository<Customer> _repository;

        public UpdateCustomerCommandHandler(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task<Result<CustomerOutputDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) return Result<CustomerOutputDto>.Error("Customer not found.");
            if (string.IsNullOrWhiteSpace(request.Name)) return Result<CustomerOutputDto>.Error("Name is required.");
            if (string.IsNullOrWhiteSpace(request.Phone)) return Result<CustomerOutputDto>.Error("Phone is required.");

            existing.Update(request.Name, request.Phone, request.Email);

            await _repository.UpdateAsync(existing, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success(existing.ToDto());
        }
    }
}
