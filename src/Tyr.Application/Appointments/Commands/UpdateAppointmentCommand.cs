using Ardalis.Result;
using MediatR;
using Tyr.Application.Appointments.Dtos;
using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Application.Appointments.Extensions;

namespace Tyr.Application.Appointments.Commands
{
    public record UpdateAppointmentCommand(Guid Id, DateTime NewStartDateTime) : IRequest<Result<AppointmentOutputDto>>;

    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, Result<AppointmentOutputDto>>
    {
        private readonly IRepository<Appointment> _appointmentRepository;

        public UpdateAppointmentCommandHandler(IRepository<Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Result<AppointmentOutputDto>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (appointment is null) return Result<AppointmentOutputDto>.Error("Appointment not found.");

            appointment.UpdateStart(request.NewStartDateTime);
            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
            await _appointmentRepository.SaveChangesAsync(cancellationToken);

            return Result.Success(appointment.ToDto());
        }
    }
}
