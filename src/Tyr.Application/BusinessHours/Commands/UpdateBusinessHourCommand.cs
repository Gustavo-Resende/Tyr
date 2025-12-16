using Ardalis.Result;
using MediatR;
using Tyr.Application.BusinessHours.Dtos;
using Tyr.Domain.BusinessHourAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.BusinessHours.Extensions;

namespace Tyr.Application.BusinessHours.Commands
{
    public record UpdateBusinessHourCommand(Guid Id, TimeSpan StartTime, TimeSpan EndTime, bool IsActive) : IRequest<Result<BusinessHourOutputDto>>;

    public class UpdateBusinessHourCommandHandler : IRequestHandler<UpdateBusinessHourCommand, Result<BusinessHourOutputDto>>
    {
        private readonly IRepository<BusinessHour> _repository;

        public UpdateBusinessHourCommandHandler(IRepository<BusinessHour> repository)
        {
            _repository = repository;
        }

        public async Task<Result<BusinessHourOutputDto>> Handle(UpdateBusinessHourCommand request, CancellationToken cancellationToken)
        {
            var bh = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (bh is null) return Result<BusinessHourOutputDto>.Error("BusinessHour not found.");
            if (request.StartTime >= request.EndTime) return Result<BusinessHourOutputDto>.Error("Start must be before end.");

            bh.Update(request.StartTime, request.EndTime, request.IsActive);
            await _repository.UpdateAsync(bh, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success(bh.ToDto());
        }
    }
}
