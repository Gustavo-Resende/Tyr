using Ardalis.Result;
using MediatR;
using Tyr.Application.BusinessHours.Dtos;
using Tyr.Domain.BusinessHourAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.BusinessHours.Extensions;

namespace Tyr.Application.BusinessHours.Queries
{
    public record GetBusinessHourByIdQuery(Guid Id) : IRequest<Result<BusinessHourOutputDto>>;

    public class GetBusinessHourByIdQueryHandler : IRequestHandler<GetBusinessHourByIdQuery, Result<BusinessHourOutputDto>>
    {
        private readonly IReadRepository<BusinessHour> _repository;

        public GetBusinessHourByIdQueryHandler(IReadRepository<BusinessHour> repository)
        {
            _repository = repository;
        }

        public async Task<Result<BusinessHourOutputDto>> Handle(GetBusinessHourByIdQuery request, CancellationToken cancellationToken)
        {
            var bh = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (bh is null) return Result<BusinessHourOutputDto>.Error("BusinessHour not found.");
            return Result.Success(bh.ToDto());
        }
    }
}
