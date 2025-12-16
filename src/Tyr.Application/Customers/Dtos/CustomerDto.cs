namespace Tyr.Application.Customers.Dtos
{
    public record CustomerInputDto(string Name, string Phone, string? Email = null);
    public record CustomerOutputDto(Guid Id, string Name, string Phone, string? Email);
}
