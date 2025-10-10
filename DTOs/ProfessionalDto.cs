using System.Security.Cryptography.X509Certificates;

namespace Tyr.DTOs
{
    public record ProfessionalDto (int Id, string? Name, string? Specialty, List<ServiceDto> Servicos);
}
