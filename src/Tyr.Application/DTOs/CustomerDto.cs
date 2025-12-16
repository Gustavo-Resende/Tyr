namespace Tyr.Application.DTOs
{
    public record CustomerInputDto(string Name, string Phone);
    public record CustomerOutputDto(int Id, string Name, string Phone);
}
