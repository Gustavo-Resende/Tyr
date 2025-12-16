namespace Tyr.Application.Services.Dtos
{
    public record ServiceDto(Guid Id, string Name, string Description, int DurationInMinutes, decimal Price);
}
