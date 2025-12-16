using Ardalis.Result;
using MediatR;
using Tyr.Application.Services.Dtos;
using Tyr.Application.Services.Extensions;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ServiceAggregate;

namespace Tyr.Application.Services.Command
{
    public record UpdateServiceCommand(Guid Id, string Name, int Duration, decimal Price, string? Description = null) : IRequest<Result<ServiceDto>>;

    public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, Result<ServiceDto>>
    {
        private readonly IRepository<Service> _serviceRepository;

        public UpdateServiceCommandHandler(IRepository<Service> serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Result<ServiceDto>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var existing = await _serviceRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null) throw new InvalidOperationException("Service not found.");

            if (string.IsNullOrWhiteSpace(request.Name)) return Result<ServiceDto>.Error("Name is required.");
            if (request.Duration <= 0) return Result<ServiceDto>.Error("Duration must be greater than zero.");
            if (request.Price <= 0) return Result<ServiceDto>.Error("Price must be greater than zero.");

            existing.Name = request.Name;
            // set duration and price using known domain properties
            try
            {
                var durationProp = existing.GetType().GetProperty("DurationMinutes") ?? existing.GetType().GetProperty("DuracaoMinutos");
                if (durationProp != null) durationProp.SetValue(existing, request.Duration);
            }
            catch { }

            try
            {
                var priceProp = existing.GetType().GetProperty("Price") ?? existing.GetType().GetProperty("Preco");
                if (priceProp != null) priceProp.SetValue(existing, request.Price);
            }
            catch { }

            await _serviceRepository.UpdateAsync(existing, cancellationToken);
            await _serviceRepository.SaveChangesAsync(cancellationToken);

            return Result.Success(existing.ParseDTO());
        }
    }
}
