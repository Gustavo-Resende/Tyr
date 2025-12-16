using Tyr.Domain.BusinessHourAggregate;
using Tyr.Application.BusinessHours.Dtos;

namespace Tyr.Application.BusinessHours.Extensions
{
    public static class BusinessHourExtension
    {
        public static BusinessHourOutputDto ToDto(this BusinessHour bh)
            => new(bh.Id, bh.DayOfWeek, bh.StartTime, bh.EndTime, bh.IsActive);

        public static IEnumerable<BusinessHourOutputDto> ToDtoList(this IEnumerable<BusinessHour> bhs)
            => bhs.Select(b => b.ToDto()).ToList();
    }
}
