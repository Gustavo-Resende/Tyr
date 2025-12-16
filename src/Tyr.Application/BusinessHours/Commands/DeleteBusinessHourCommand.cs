using Ardalis.Result;
using MediatR;
using Tyr.Domain.BusinessHourAggregate;
using Tyr.Domain.Interfaces;

namespace Tyr.Application.BusinessHours.Commands
{
    public record DeleteBusinessHourCommand(Guid Id) : IRequest<Result>;

    public class DeleteBusinessHourCommandHandler : IRequestHandler<DeleteBusinessHourCommand, Result>
    {
        private readonly IRepository<BusinessHour> _repository;

        public DeleteBusinessHourCommandHandler(IRepository<BusinessHour> repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteBusinessHourCommand request, CancellationToken cancellationToken)
        {
            var bh = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (bh is null) return Result.Error("BusinessHour not found.");

            await _repository.DeleteAsync(bh, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
