using MediatR;
using Tyr.Application.Customers.Commands;
using Tyr.Application.Customers.Dtos;
using Tyr.Application.Customers.Extensions;
using Tyr.Application.Customers.Queries;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.Interfaces;

namespace Tyr.Endpoints
{
    public static class CustomerEndpoint
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/customers", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetAllCustomersQuery(), cancellationToken);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.Problem(result.Errors?.FirstOrDefault());
            });

            app.MapPost("/customers", async (IMediator mediator, CustomerInputDto customerInputDto, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new CreateCustomerCommand(customerInputDto.Name, customerInputDto.Phone, customerInputDto.Email), cancellationToken);
                if (res.IsSuccess) return Results.Created($"/customers/{res.Value.Id}", res.Value);
                return Results.BadRequest(res.Errors?.FirstOrDefault());
            });

            app.MapDelete("/customers/{id}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new DeleteCustomerCommand(id), cancellationToken);
                if (res.IsSuccess) return Results.NoContent();
                return Results.NotFound();
            });

            app.MapGet("/customers/{id}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new GetCustomerByIdQuery(id), cancellationToken);
                if (res.IsSuccess) return Results.Ok(res.Value);
                return Results.NotFound();
            });

            app.MapPut("/customers/{id}", async (IMediator mediator, Guid id, CustomerInputDto customerInputDto, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new UpdateCustomerCommand(id, customerInputDto.Name, customerInputDto.Phone, customerInputDto.Email), cancellationToken);
                if (res.IsSuccess) return Results.Ok(res.Value);
                return Results.NotFound();
            });
        }
    }
}
