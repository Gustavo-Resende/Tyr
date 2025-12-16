using MediatR;
using Tyr.Application.Services.Command;
using Tyr.Application.Services.Dtos;
using Tyr.Application.Services.Extensions;
using Tyr.Application.Services.Queries;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ServiceAggregate;

namespace Tyr.Endpoints
{
    public static class ServiceEndpoints
    {
        public static void MapServiceEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/services", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new GetAllServiceQuery(), cancellationToken);
                return res.IsSuccess ? Results.Ok(res.Value) : Results.Problem(res.Errors?.FirstOrDefault());
            }).WithName("GetServices").WithTags("Services").WithOpenApi(operation =>
            {
                operation.Summary = "Listar serviços";
                operation.Description = "Retorna todos os serviços disponíveis.";
                return operation;
            });

            app.MapPost("/services", async (IMediator mediator, ServiceDto request, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new CreateServiceCommand(request.Name, request.Description, request.DurationInMinutes, request.Price), cancellationToken);
                if (res.IsSuccess) return Results.Created($"/services/{res.Value.Id}", res.Value);
                return Results.BadRequest(res.Errors?.FirstOrDefault());
            }).WithName("CreateService").WithTags("Services").WithOpenApi(operation =>
            {
                operation.Summary = "Criar serviço";
                operation.Description = "Cria um novo serviço com duração e preço.";
                return operation;
            });

            app.MapGet("/services/{id}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new GetServiceByIdQuery(id), cancellationToken);
                if (res.IsSuccess) return Results.Ok(res.Value);
                return Results.NotFound();
            }).WithName("GetServiceById").WithTags("Services").WithOpenApi(operation =>
            {
                operation.Summary = "Obter serviço por Id";
                operation.Description = "Retorna detalhes de um serviço específico.";
                return operation;
            });

            app.MapPut("/services/{id}", async (IMediator mediator, Guid id, ServiceDto servico, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new UpdateServiceCommand(id, servico.Name, servico.DurationInMinutes, servico.Price, servico.Description), cancellationToken);
                if (res.IsSuccess) return Results.Ok(res.Value);
                return Results.NotFound();
            }).WithName("UpdateService").WithTags("Services").WithOpenApi(operation =>
            {
                operation.Summary = "Atualizar serviço";
                operation.Description = "Atualiza os dados de um serviço existente.";
                return operation;
            });

            app.MapDelete("/services/{id}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
            {
                var res = await mediator.Send(new DeleteServiceCommand(id), cancellationToken);
                if (res.IsSuccess) return Results.NoContent();
                return Results.NotFound();
            }).WithName("DeleteService").WithTags("Services").WithOpenApi(operation =>
            {
                operation.Summary = "Remover serviço";
                operation.Description = "Remove um serviço pelo Id.";
                return operation;
            });
        }
    }
}
