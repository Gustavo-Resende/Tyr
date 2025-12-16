using Ardalis.Result;
using MediatR;
using Tyr.Application.Services.Dtos;
using Tyr.Domain.Interfaces;
using Tyr.Application.Services.Extensions;
using Tyr.Domain.ServiceAggregate;

namespace Tyr.Application.Services.Queries
{
    public record GetAllServiceQuery : IRequest<Result<IReadOnlyList<ServiceDto>>>;

    public class GetAllServiceQueryHandler : IRequestHandler<GetAllServiceQuery, Result<IReadOnlyList<ServiceDto>>>
    {
        private readonly IReadRepository<Service> _serviceRepository;

        public GetAllServiceQueryHandler(IReadRepository<Service> serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Result<IReadOnlyList<ServiceDto>>> Handle(GetAllServiceQuery request, CancellationToken cancellationToken)
        {
            var services = await _serviceRepository.ListAsync(cancellationToken);
            var dtos = services.Select(s => s.ParseDTO()).ToList();
            return Result.Success<IReadOnlyList<ServiceDto>>(dtos.AsReadOnly());
        }
    }
}
