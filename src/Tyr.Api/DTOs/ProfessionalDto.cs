namespace Tyr.DTOs
{
    public record ProfissionalDto(int Id, string? Nome, List<ServicoOutputDto> Servicos);
}
