using Ardalis.Result;
using MediatR;
using Tyr.Application.BusinessHours.Dtos;
using Tyr.Domain.BusinessHourAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.BusinessHours.Extensions;

namespace Tyr.Application.BusinessHours.Commands
{
    public record CreateBusinessHourCommand(DayOfWeek DayOfWeek, TimeSpan StartTime, TimeSpan EndTime, bool IsActive) : IRequest<Result<BusinessHourOutputDto>>;

    public class CreateBusinessHourCommandHandler : IRequestHandler<CreateBusinessHourCommand, Result<BusinessHourOutputDto>>
    {
        private readonly IRepository<BusinessHour> _repository;

        public CreateBusinessHourCommandHandler(IRepository<BusinessHour> repository)
        {
            _repository = repository;
        }

        public async Task<Result<BusinessHourOutputDto>> Handle(CreateBusinessHourCommand request, CancellationToken cancellationToken)
        {
            if (request.StartTime >= request.EndTime) return Result<BusinessHourOutputDto>.Error("Start must be before end.");

            var bh = new BusinessHour(request.DayOfWeek, request.StartTime, request.EndTime, request.IsActive);
            await _repository.AddAsync(bh, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success(bh.ToDto());
        }
    }
}
