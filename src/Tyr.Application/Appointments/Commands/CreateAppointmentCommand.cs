using Ardalis.Result;
using MediatR;
using Tyr.Application.Appointments.Dtos;
using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ServiceAggregate;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.BusinessHourAggregate;
using Tyr.Domain.AppointmentAggregate.Specifications;
using Tyr.Application.Appointments.Extensions;

namespace Tyr.Application.Appointments.Commands
{
    public record CreateAppointmentCommand(DateTime StartDateTime, Guid CustomerId, Guid ServiceId, string? Notes) : IRequest<Result<AppointmentOutputDto>>;

    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<AppointmentOutputDto>>
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IReadRepository<Service> _serviceRepository;
        private readonly IReadRepository<Customer> _customerRepository;
        private readonly IReadRepository<BusinessHour> _businessHourRepository;

        public CreateAppointmentCommandHandler(IRepository<Appointment> appointmentRepository,
            IReadRepository<Service> serviceRepository,
            IReadRepository<Customer> customerRepository,
            IReadRepository<BusinessHour> businessHourRepository)
        {
            _appointmentRepository = appointmentRepository;
            _serviceRepository = serviceRepository;
            _customerRepository = customerRepository;
            _businessHourRepository = businessHourRepository;
        }

        public async Task<Result<AppointmentOutputDto>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            // Validate customer
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer is null) return Result<AppointmentOutputDto>.Error("Customer not found.");

            // Validate service
            var service = await _serviceRepository.GetByIdAsync(request.ServiceId, cancellationToken);
            if (service is null) return Result<AppointmentOutputDto>.Error("Service not found.");

            // Check business hours
            var dayOfWeek = request.StartDateTime.DayOfWeek;
            var bh = (await _businessHourRepository.ListAsync(cancellationToken)).FirstOrDefault(b => b.DayOfWeek == dayOfWeek && b.IsActive);
            if (bh is null) return Result<AppointmentOutputDto>.Error("Business is closed on that day.");

            var timeOfDay = request.StartDateTime.TimeOfDay;
            if (timeOfDay < bh.StartTime || timeOfDay >= bh.EndTime) return Result<AppointmentOutputDto>.Error("Start time outside business hours.");

            // Calculate end time
            var end = request.StartDateTime.AddMinutes(service.DurationInMinutes);

            // Check conflict
            var conflictSpec = new AppointmentConflictSpec(service.Id, request.StartDateTime, end);
            var conflicts = await _appointmentRepository.ListAsync(conflictSpec, cancellationToken);
            if (conflicts.Any()) return Result<AppointmentOutputDto>.Error("Time slot is not available.");

            var appointment = new Appointment(request.CustomerId, request.ServiceId, request.StartDateTime, request.Notes);
            appointment.CalculateEndDateTime(service.DurationInMinutes);

            await _appointmentRepository.AddAsync(appointment, cancellationToken);
            await _appointmentRepository.SaveChangesAsync(cancellationToken);

            return Result.Success(appointment.ToDto());
        }
    }
}
