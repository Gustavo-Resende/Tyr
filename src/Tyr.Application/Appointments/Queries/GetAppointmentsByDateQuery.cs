using Ardalis.Result;
using MediatR;
using Tyr.Application.Appointments.Dtos;
using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.Appointments.Extensions;

namespace Tyr.Application.Appointments.Queries
{
    public record GetAppointmentsByDateQuery(DateTime Date) : IRequest<Result<IReadOnlyList<AppointmentOutputDto>>>;

    public class GetAppointmentsByDateQueryHandler : IRequestHandler<GetAppointmentsByDateQuery, Result<IReadOnlyList<AppointmentOutputDto>>>
    {
        private readonly IReadRepository<Appointment> _appointmentRepository;

        public GetAppointmentsByDateQueryHandler(IReadRepository<Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Result<IReadOnlyList<AppointmentOutputDto>>> Handle(GetAppointmentsByDateQuery request, CancellationToken cancellationToken)
        {
            var start = request.Date.Date;
            var end = start.AddDays(1);
            var all = await _appointmentRepository.ListAsync(cancellationToken);
            var list = all.Where(a => a.StartDateTime >= start && a.StartDateTime < end).ToList();
            var dtos = list.Select(a => a.ToDto()).ToList();
            return Result.Success<IReadOnlyList<AppointmentOutputDto>>(dtos.AsReadOnly());
        }
    }
}
