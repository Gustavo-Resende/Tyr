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
            });

            app.MapPost("/businesshours", async (IMediator mediator, BusinessHourInputDto dto, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new CreateBusinessHourCommand(dto.DayOfWeek, dto.StartTime, dto.EndTime, dto.IsActive), cancellationToken);
                if (res.IsSuccess) return Results.Created($"/businesshours/{res.Value.Id}", res.Value);
                return Results.BadRequest(res.Errors?.FirstOrDefault());
            });

            app.MapGet("/businesshours/{id}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new GetBusinessHourByIdQuery(id), cancellationToken);
                if (res.IsSuccess) return Results.Ok(res.Value);
                return Results.NotFound();
            });

            app.MapPut("/businesshours/{id}", async (IMediator mediator, Guid id, BusinessHourInputDto dto, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new UpdateBusinessHourCommand(id, dto.StartTime, dto.EndTime, dto.IsActive), cancellationToken);
                if (res.IsSuccess) return Results.Ok(res.Value);
                return Results.NotFound();
            });

            app.MapDelete("/businesshours/{id}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new DeleteBusinessHourCommand(id), cancellationToken);
                if (res.IsSuccess) return Results.NoContent();
                return Results.NotFound();
            });
        }
    }
}
