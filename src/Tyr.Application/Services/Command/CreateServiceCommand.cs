using Ardalis.Result;
using MediatR;
using Tyr.Application.Services.Dtos;
using Tyr.Application.Services.Extensions;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ServiceAggregate;

namespace Tyr.Application.Services.Command
{
    public record CreateServiceCommand(string Name, string? Description, int DurationInMinutes, decimal Price) : IRequest<Result<ServiceDto>>;

    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Result<ServiceDto>>
    {
        private readonly IRepository<Service> _serviceRepository;
        public CreateServiceCommandHandler(IRepository<Service> serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Result<ServiceDto>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) return Result<ServiceDto>.Error("Name is required.");
            if (request.DurationInMinutes <= 0) return Result<ServiceDto>.Error("Duration must be greater than zero.");
            if (request.Price <= 0) return Result<ServiceDto>.Error("Price must be greater than zero.");

            var newService = new Service(request.Name, request.Price, request.DurationInMinutes, request.Description);
            await _serviceRepository.AddAsync(newService, cancellationToken);
            await _serviceRepository.SaveChangesAsync(cancellationToken);

            return Result.Success(newService.ParseDTO());
        }
    }
}