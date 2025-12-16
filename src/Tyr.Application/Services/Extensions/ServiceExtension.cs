using Tyr.Application.Services.Dtos;
using Tyr.Domain.ServiceAggregate;

namespace Tyr.Application.Services.Extensions
{
    public static class ServiceExtension
    {
        public static ServiceDto ParseDTO(this Service service)
            => new(service.Id, service.Name, service.Price, service.DurationMinutes);

        public static List<ServiceDto> ParseDTOList(this List<Service> services)
            => services.Select(s => s.ParseDTO()).ToList();
    }
}
