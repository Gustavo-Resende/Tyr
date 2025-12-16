using Tyr.Domain.AppointmentAggregate;
using Tyr.Domain.AppointmentAggregate.Specifications;
using Tyr.Domain.Interfaces;
using Tyr.Application.Extensions;

namespace Tyr.Endpoints
{
    public static class AppointmentEndpoint
    {
        public static void MapAppointmentEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/appointments", async (IReadRepository<Appointment> repository, CancellationToken cancellationToken) =>
            {
                var spec = new AppointmentsWithDetailsSpec();

                var appointments = await repository.ListAsync(spec, cancellationToken);

                return Results.Ok(appointments.ParseDTOList());
            });

            // Example (commented): create appointment flow translated to English
            // app.MapPost("/appointments", async (AppointmentInputDto inputDto, IAppointmentService repository, CancellationToken cancellationToken) =>
            // {
            //     var customerExists = await repository.CheckCustomerExistsAsync(inputDto.CustomerId);
            //     if (!customerExists) return Results.NotFound();
            //
            //     var professionalExists = await repository.CheckProfessionalExistsAsync(inputDto.ProfessionalId);
            //     if (!professionalExists) return Results.NotFound();
            //
            //     var serviceExists = await repository.CheckServiceExistsAsync(inputDto.ServiceId);
            //     if (!serviceExists) return Results.NotFound();
            //
            //     var serviceDuration = TimeSpan.FromMinutes(30);
            //     var startTime = inputDto.StartTime.Value;
            //     var professionalId = inputDto.ProfessionalId;
            //
            //     var conflictSpec = new AppointmentConflictSpec(professionalId, startTime, serviceDuration);
            //
            //     // check conflict using repository/spec
            //     DateTimeOffset newEndTime = inputDto.StartTime.Value.Add(serviceDuration);
            //
            //     var conflictFound = await context.Appointments.AnyAsync(a =>
            //         a.ProfessionalId == inputDto.ProfessionalId &&
            //         a.StartTime.HasValue && a.Duration.HasValue &&
            //         inputDto.StartTime < (a.StartTime.Value + a.Duration.Value) &&
            //         newEndTime > a.StartTime.Value
            //     );
            //
            //     if (conflictFound) return Results.Conflict("Time not available for this professional.");
            //
            //     var appointment = new Appointment
            //     {
            //         StartTime = inputDto.StartTime,
            //         CustomerId = inputDto.CustomerId,
            //         ProfessionalId = inputDto.ProfessionalId,
            //         ServiceId = inputDto.ServiceId,
            //     };
            //
            //     context.Appointments.Add(appointment);
            //     await context.SaveChangesAsync();
            //
            //     var outputDto = new AppointmentOutputDto(
            //         appointment.Id,
            //         appointment.StartTime,
            //         appointment.Status,
            //         customerExists.Name,
            //         professionalExists.Name,
            //         serviceExists.Name
            //     );
            //
            //     return Results.Created($"/appointments/{appointment.Id}", outputDto);
            // });

            app.MapDelete("/appointments/{id}", async (int id, IRepository<Appointment> repository, CancellationToken cancellationToken) =>
            {
                var appointment = await repository.GetByIdAsync(id, cancellationToken);
                if (appointment == null)
                {
                    return Results.NotFound();
                }

                await repository.DeleteAsync(appointment, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });
        }
    }
}
