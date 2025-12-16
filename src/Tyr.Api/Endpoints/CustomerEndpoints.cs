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
            }).WithName("GetCustomers").WithTags("Customers").WithOpenApi(operation =>
            {
                operation.Summary = "Listar clientes";
                operation.Description = "Retorna a lista completa de clientes cadastrados.";
                return operation;
            });

            app.MapPost("/customers", async (IMediator mediator, CustomerInputDto customerInputDto, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new CreateCustomerCommand(customerInputDto.Name, customerInputDto.Phone, customerInputDto.Email), cancellationToken);
                if (res.IsSuccess) return Results.Created($"/customers/{res.Value.Id}", res.Value);
                return Results.BadRequest(res.Errors?.FirstOrDefault());
            }).WithName("CreateCustomer").WithTags("Customers").WithOpenApi(operation =>
            {
                operation.Summary = "Criar cliente";
                operation.Description = "Cria um novo cliente usando nome, telefone e e-mail opcionais.";
                return operation;
            });

            app.MapDelete("/customers/{id}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new DeleteCustomerCommand(id), cancellationToken);
                if (res.IsSuccess) return Results.NoContent();
                return Results.NotFound();
            }).WithName("DeleteCustomer").WithTags("Customers").WithOpenApi(operation =>
            {
                operation.Summary = "Remover cliente";
                operation.Description = "Remove um cliente pelo Id. Não remove se houver dependências.";
                return operation;
            });

            app.MapGet("/customers/{id}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new GetCustomerByIdQuery(id), cancellationToken);
                if (res.IsSuccess) return Results.Ok(res.Value);
                return Results.NotFound();
            }).WithName("GetCustomerById").WithTags("Customers").WithOpenApi(operation =>
            {
                operation.Summary = "Obter cliente por Id";
                operation.Description = "Retorna os dados de um cliente específico identificado pelo Id.";
                return operation;
            });

            app.MapPut("/customers/{id}", async (IMediator mediator, Guid id, CustomerInputDto customerInputDto, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new UpdateCustomerCommand(id, customerInputDto.Name, customerInputDto.Phone, customerInputDto.Email), cancellationToken);
                if (res.IsSuccess) return Results.Ok(res.Value);
                return Results.NotFound();
            }).WithName("UpdateCustomer").WithTags("Customers").WithOpenApi(operation =>
            {
                operation.Summary = "Atualizar cliente";
                operation.Description = "Atualiza dados de um cliente existente.";
                return operation;
            });
        }
    }
}
