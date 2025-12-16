using Ardalis.Result;
using MediatR;
using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.Interfaces;

namespace Tyr.Application.Appointments.Commands
{
    public record DeleteAppointmentCommand(System.Guid Id) : IRequest<Result>;

    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, Result>
    {
        private readonly IRepository<Appointment> _repository;

        public DeleteAppointmentCommandHandler(IRepository<Appointment> repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appt = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (appt is null) return Result.Error("Appointment not found.");

            await _repository.DeleteAsync(appt, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
