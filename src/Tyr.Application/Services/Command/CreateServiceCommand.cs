using Ardalis.Result;
using MediatR;
using Tyr.Application.Services.Dtos;
using Tyr.Application.Services.Extensions;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ServiceAggregate;

namespace Tyr.Application.Services.Command
{
    public record CreateServiceCommand(string Name, decimal Price, int Time) : IRequest<Result<ServiceDto>>;

    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Result<ServiceDto>>
    {
        private readonly IRepository<Service> _serviceRepository;
        public CreateServiceCommandHandler(IRepository<Service> serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Result<ServiceDto>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var newService = new Service(request.Name, request.Price, request.Time);
            await _serviceRepository.AddAsync(newService, cancellationToken);
            await _serviceRepository.SaveChangesAsync(cancellationToken);

            return Result.Success(newService.ParseDTO());
        }
    }
}