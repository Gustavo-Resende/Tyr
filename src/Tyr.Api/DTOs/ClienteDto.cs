namespace Tyr.DTOs
{
    public record ClienteInputDto(string Nome, string Telefone);
    public record ClienteOutputDto(int Id, string Nome, string Telefone);
}
