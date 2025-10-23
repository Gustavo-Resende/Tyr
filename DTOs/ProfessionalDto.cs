using System.Security.Cryptography.X509Certificates;

namespace Tyr.DTOs
{
    public record ProfissionalDto (int Id, string? Nome, List<ServicoDto> Servicos);
}
