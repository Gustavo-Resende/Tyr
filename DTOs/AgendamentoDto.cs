using Tyr.Models;

namespace Tyr.DTOs
{
    public record AgendamentoInputDto(DateTimeOffset? Horario, int ClienteId, int ProfissionalId, int ServicoId);
    public record AgendamentoOutputDto(
        int Id,
        DateTimeOffset? Horario,
        string? Status,
        string? ClienteNome,
        string? ProfissionalNome,
        string? ServicoNome
    );
}
