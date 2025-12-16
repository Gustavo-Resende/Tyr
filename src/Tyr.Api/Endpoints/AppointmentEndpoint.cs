using MediatR;
using Tyr.Application.Appointments.Commands;
using Tyr.Application.Appointments.Dtos;
using Tyr.Application.Appointments.Queries;

namespace Tyr.Endpoints
{
    public static class AppointmentEndpoint
    {
        public static void MapAppointmentEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/appointments", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new GetAllAppointmentsQuery(), cancellationToken);
                return res.IsSuccess ? Results.Ok(res.Value) : Results.Problem(res.Errors?.FirstOrDefault());
            }).WithName("GetAppointments").WithTags("Appointments").WithOpenApi(operation =>
            {
                operation.Summary = "Listar agendamentos";
                operation.Description = "Retorna todos os agendamentos com detalhes de cliente e serviço.";
                return operation;
            });

            app.MapPost("/appointments", async (IMediator mediator, AppointmentInputDto dto, CancellationToken cancellationToken) =>
            {
                var cmd = new CreateAppointmentCommand(dto.StartDateTime, dto.CustomerId, dto.ServiceId, dto.Notes);
                var res = await mediator.Send(cmd, cancellationToken);
                if (res.IsSuccess) return Results.Created($"/appointments/{res.Value.Id}", res.Value);
                return Results.BadRequest(res.Errors?.FirstOrDefault());
            }).WithName("CreateAppointment").WithTags("Appointments").WithOpenApi(operation =>
            {
                operation.Summary = "Criar agendamento";
                operation.Description = "Cria um novo agendamento verificando disponibilidade e horário comercial.";
                return operation;
            });

            app.MapDelete("/appointments/{id}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new DeleteAppointmentCommand(id), cancellationToken);
                if (res.IsSuccess) return Results.NoContent();
                return Results.NotFound();
            }).WithName("DeleteAppointment").WithTags("Appointments").WithOpenApi(operation =>
            {
                operation.Summary = "Remover agendamento";
                operation.Description = "Remove um agendamento existente pelo Id.";
                return operation;
            });
        }
    }
}
