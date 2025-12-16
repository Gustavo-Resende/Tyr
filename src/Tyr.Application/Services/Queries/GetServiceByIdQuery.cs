using Ardalis.Result;
using MediatR;
using Tyr.Application.Services.Dtos;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ServiceAggregate;
using Tyr.Application.Services.Extensions;

namespace Tyr.Application.Services.Queries
{
    public record GetServiceByIdQuery(Guid Id) : IRequest<Result<ServiceDto>>;

    public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, Result<ServiceDto>>
    {
        private readonly IReadRepository<Service> _repository;

        public GetServiceByIdQueryHandler(IReadRepository<Service> repository)
        {
            _repository = repository;
        }

        public async Task<Result<ServiceDto>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var service = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (service is null) return Result<ServiceDto>.Error("Service not found.");
            return Result.Success(service.ParseDTO());
        }
    }
}
