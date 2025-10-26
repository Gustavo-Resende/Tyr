namespace Tyr.DTOs
{
    public record ServicoInputDto(string Nome, decimal Preco, int ProfissionalId);
    public record ServicoOutputDto(int Id, string Nome, decimal Preco, int ProfissionalId);
}
