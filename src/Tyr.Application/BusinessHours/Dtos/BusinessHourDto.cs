namespace Tyr.Application.BusinessHours.Dtos
{
    public record BusinessHourInputDto(DayOfWeek DayOfWeek, TimeSpan StartTime, TimeSpan EndTime, bool IsActive = true);

    public record BusinessHourOutputDto(Guid Id, DayOfWeek DayOfWeek, TimeSpan StartTime, TimeSpan EndTime, bool IsActive);
}
