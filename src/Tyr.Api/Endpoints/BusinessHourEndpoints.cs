using MediatR;
using Tyr.Application.BusinessHours.Commands;
using Tyr.Application.BusinessHours.Dtos;
using Tyr.Application.BusinessHours.Extensions;
using Tyr.Application.BusinessHours.Queries;
using Tyr.Domain.BusinessHourAggregate;
using Tyr.Domain.Interfaces;

namespace Tyr.Endpoints
{
    public static class BusinessHourEndpoints
    {
        public static void MapBusinessHourEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/businesshours", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new GetAllBusinessHoursQuery(), cancellationToken);
                return res.IsSuccess ? Results.Ok(res.Value) : Results.Problem(res.Errors?.FirstOrDefault());
            }).WithName("GetBusinessHours").WithTags("BusinessHours").WithOpenApi(operation =>
            {
                operation.Summary = "Listar horário de funcionamento";
                operation.Description = "Retorna os horários comerciais configurados.";
                return operation;
            });

            app.MapGet("/businesshours/available", async (IMediator mediator, DayOfWeek dayOfWeek, Guid? serviceId, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new GetAvailableSlotsByDayQuery(dayOfWeek, serviceId), cancellationToken);
                return res.IsSuccess ? Results.Ok(res.Value) : Results.Problem(res.Errors?.FirstOrDefault());
            }).WithName("GetAvailableSlotsByDay").WithTags("BusinessHours").WithOpenApi(operation =>
            {
                operation.Summary = "Obter horários disponíveis";
                operation.Description = "Retorna os horários disponíveis para um dia da semana. Parâmetros: dayOfWeek (required), serviceId (optional).";
                return operation;
            });

            app.MapPost("/businesshours", async (IMediator mediator, BusinessHourInputDto dto, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new CreateBusinessHourCommand(dto.DayOfWeek, dto.StartTime, dto.EndTime, dto.IsActive), cancellationToken);
                if (res.IsSuccess) return Results.Created($"/businesshours/{res.Value.Id}", res.Value);
                return Results.BadRequest(res.Errors?.FirstOrDefault());
            }).WithName("CreateBusinessHour").WithTags("BusinessHours").WithOpenApi(operation =>
            {
                operation.Summary = "Criar horário";
                operation.Description = "Adiciona um horário de funcionamento para um dia da semana.";
                return operation;
            });

            app.MapGet("/businesshours/{id}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new GetBusinessHourByIdQuery(id), cancellationToken);
                if (res.IsSuccess) return Results.Ok(res.Value);
                return Results.NotFound();
            }).WithName("GetBusinessHourById").WithTags("BusinessHours").WithOpenApi(operation =>
            {
                operation.Summary = "Obter horário por Id";
                operation.Description = "Retorna um horário de funcionamento específico.";
                return operation;
            });

            app.MapPut("/businesshours/{id}", async (IMediator mediator, Guid id, BusinessHourInputDto dto, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new UpdateBusinessHourCommand(id, dto.StartTime, dto.EndTime, dto.IsActive), cancellationToken);
                if (res.IsSuccess) return Results.Ok(res.Value);
                return Results.NotFound();
            }).WithName("UpdateBusinessHour").WithTags("BusinessHours").WithOpenApi(operation =>
            {
                operation.Summary = "Atualizar horário";
                operation.Description = "Atualiza um horário de funcionamento existente.";
                return operation;
            });

            app.MapDelete("/businesshours/{id}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new DeleteBusinessHourCommand(id), cancellationToken);
                if (res.IsSuccess) return Results.NoContent();
                return Results.NotFound();
            }).WithName("DeleteBusinessHour").WithTags("BusinessHours").WithOpenApi(operation =>
            {
                operation.Summary = "Remover horário";
                operation.Description = "Remove um horário de funcionamento pelo Id.";
                return operation;
            });
        }
    }
}
