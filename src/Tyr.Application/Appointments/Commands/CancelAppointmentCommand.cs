using Ardalis.Result;
using MediatR;
using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.Interfaces;

namespace Tyr.Application.Appointments.Commands
{
    public record CancelAppointmentCommand(Guid Id) : IRequest<Result>;

    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, Result>
    {
        private readonly IRepository<Appointment> _appointmentRepository;

        public CancelAppointmentCommandHandler(IRepository<Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Result> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (appointment is null) return Result.Error("Appointment not found.");

            appointment.UpdateStatus(AppointmentStatus.Canceled);
            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
            await _appointmentRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
