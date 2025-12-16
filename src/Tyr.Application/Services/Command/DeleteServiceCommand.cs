using Ardalis.Result;
using MediatR;
using System.Collections;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ServiceAggregate;

namespace Tyr.Application.Services.Command
{
    public record DeleteServiceCommand(Guid Id) : IRequest<Result>;

    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, Result>
    {
        private readonly IRepository<Service> _service_repository;

        public DeleteServiceCommandHandler(IRepository<Service> service_repository)
        {
            _service_repository = service_repository;
        }

        public async Task<Result> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var existing = await _service_repository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) return Result.NotFound();

            try
            {
                var nav = existing.GetType().GetProperty("Appointments");
                if (nav != null)
                {
                    var list = nav.GetValue(existing) as ICollection;
                    if (list != null && list.Count > 0) return Result.Conflict("Service has appointments and cannot be deleted.");
                }
            }
            catch { }

            await _service_repository.DeleteAsync(existing, cancellationToken);
            await _service_repository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
