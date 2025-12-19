using Ardalis.Result;
using MediatR;
using Tyr.Domain.Interfaces;
using Tyr.Domain.BusinessHourAggregate;
using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.ServiceAggregate;
using Tyr.Domain.AppointmentAggregate.Specifications;

namespace Tyr.Application.BusinessHours.Queries
{
    public record GetAvailableSlotsByDayQuery(DayOfWeek DayOfWeek, Guid? ServiceId = null) : IRequest<Result<IReadOnlyList<TimeSpan>>>;

    public class GetAvailableSlotsByDayQueryHandler : IRequestHandler<GetAvailableSlotsByDayQuery, Result<IReadOnlyList<TimeSpan>>>
    {
        private readonly IReadRepository<BusinessHour> _bhRepo;
        private readonly IReadRepository<Appointment> _appointmentRepo;
        private readonly IReadRepository<Service> _serviceRepo;

        public GetAvailableSlotsByDayQueryHandler(IReadRepository<BusinessHour> bhRepo,
            IReadRepository<Appointment> appointmentRepo,
            IReadRepository<Service> serviceRepo)
        {
            _bhRepo = bhRepo;
            _appointmentRepo = appointmentRepo;
            _serviceRepo = serviceRepo;
        }

        private static DateTime TruncateToMinutes(DateTime dt)
            => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, DateTimeKind.Utc);

        public async Task<Result<IReadOnlyList<TimeSpan>>> Handle(GetAvailableSlotsByDayQuery request, CancellationToken cancellationToken)
        {
            // find active business hour for the given day
            var bhs = await _bhRepo.ListAsync(cancellationToken);
            var bh = bhs.FirstOrDefault(b => b.DayOfWeek == request.DayOfWeek && b.IsActive);
            if (bh is null) return Result<IReadOnlyList<TimeSpan>>.Error("Business is closed on that day.");

            // determine date for the requested day (next occurrence from today)
            var date = DateTime.UtcNow.Date;
            while (date.DayOfWeek != request.DayOfWeek)
                date = date.AddDays(1);

            var dayStart = DateTime.SpecifyKind(date.Add(bh.StartTime), DateTimeKind.Utc);
            var dayEnd = DateTime.SpecifyKind(date.Add(bh.EndTime), DateTimeKind.Utc);

            int durationMinutes = 30; // default slot length (max)
            if (request.ServiceId.HasValue)
            {
                var service = await _serviceRepo.GetByIdAsync(request.ServiceId.Value, cancellationToken);
                if (service is null) return Result<IReadOnlyList<TimeSpan>>.Error("Service not found.");
                durationMinutes = Math.Min(service.DurationInMinutes, 30);
            }

            // expand query window slightly to ensure retrieval of appointments that may overlap start
            var spec = new AppointmentsByDateSpec(dayStart.AddMinutes(-durationMinutes), dayEnd);
            var appointments = (await _appointmentRepo.ListAsync(spec, cancellationToken)).ToList();

            // normalize appointment windows to UTC and truncate to minutes for robust comparisons
            //var normalizedAppointments = appointments.Select(a =>
            //{
            //    var aStart = DateTime.SpecifyKind(a.StartDateTime, DateTimeKind.Utc);
            //    var aEnd = a.EndDateTime == default ? a.StartDateTime.AddMinutes(durationMinutes) : a.EndDateTime;
            //    aEnd = DateTime.SpecifyKind(aEnd, DateTimeKind.Utc);
            //    return (Start: TruncateToMinutes(aStart), End: TruncateToMinutes(aEnd));
            //}).ToList();

            var normalizedAppointments = appointments.Select(a =>
            {
                var aStartUtc = DateTime.SpecifyKind(a.StartDateTime, DateTimeKind.Utc).ToUniversalTime();
                var aEndUtc = a.EndDateTime == default
                    ? aStartUtc.AddMinutes(durationMinutes)
                    : DateTime.SpecifyKind(a.EndDateTime, DateTimeKind.Utc).ToUniversalTime();

                // guard: ensure end is after start
                if (aEndUtc <= aStartUtc) aEndUtc = aStartUtc.AddMinutes(durationMinutes);

                return (Start: aStartUtc, End: aEndUtc);
            }).ToList();

            var slots = new List<TimeSpan>();

            var step = TimeSpan.FromMinutes(15);
            for (var slotStart = dayStart; slotStart.AddMinutes(durationMinutes) <= dayEnd; slotStart = slotStart.Add(step))
            {
                var slotEnd = slotStart.AddMinutes(durationMinutes);
                var slotStartTrunc = TruncateToMinutes(slotStart);
                var slotEndTrunc = TruncateToMinutes(slotEnd);

                var conflict = normalizedAppointments.Any(a => a.Start < slotEndTrunc && a.End > slotStartTrunc);

                if (!conflict)
                {
                    slots.Add(slotStart.TimeOfDay);
                }
            }

            return Result.Success<IReadOnlyList<TimeSpan>>(slots.AsReadOnly());
        }
    }
}
