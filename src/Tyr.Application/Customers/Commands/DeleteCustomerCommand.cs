using Ardalis.Result;
using MediatR;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.Customers.Extensions;

namespace Tyr.Application.Customers.Commands
{
    public record DeleteCustomerCommand(Guid Id) : IRequest<Result>;

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly IRepository<Customer> _repository;

        public DeleteCustomerCommandHandler(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) return Result.Error("Customer not found.");

            // check appointments - if exists, return conflict
            try
            {
                var nav = existing.GetType().GetProperty("Appointments");
                if (nav != null)
                {
                    var list = nav.GetValue(existing) as System.Collections.ICollection;
                    if (list != null && list.Count > 0) return Result.Conflict("Customer has appointments and cannot be deleted.");
                }
            }
            catch { }

            await _repository.DeleteAsync(existing, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
