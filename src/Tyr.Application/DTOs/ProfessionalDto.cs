using Tyr.Application.Services.Dtos;

namespace Tyr.Application.DTOs
{
    public record ProfissionalDto(int Id, string? Nome, List<ServicoOutputDto> Servicos);
}
