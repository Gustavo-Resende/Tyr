using Ardalis.Result;
using MediatR;
using Tyr.Application.BusinessHours.Dtos;
using Tyr.Domain.BusinessHourAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.BusinessHours.Extensions;

namespace Tyr.Application.BusinessHours.Queries
{
    public record GetAllBusinessHoursQuery : IRequest<Result<IReadOnlyList<BusinessHourOutputDto>>>;

    public class GetAllBusinessHoursQueryHandler : IRequestHandler<GetAllBusinessHoursQuery, Result<IReadOnlyList<BusinessHourOutputDto>>>
    {
        private readonly IReadRepository<BusinessHour> _repository;

        public GetAllBusinessHoursQueryHandler(IReadRepository<BusinessHour> repository)
        {
            _repository = repository;
        }

        public async Task<Result<IReadOnlyList<BusinessHourOutputDto>>> Handle(GetAllBusinessHoursQuery request, CancellationToken cancellationToken)
        {
            var list = await _repository.ListAsync(cancellationToken);
            var dtos = list.Select(b => b.ToDto()).ToList();
            return Result.Success<IReadOnlyList<BusinessHourOutputDto>>(dtos.AsReadOnly());
        }
    }
}
