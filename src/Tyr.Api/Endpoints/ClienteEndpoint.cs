using Tyr.Api.Extensions;
using Tyr.Application.DTOs;
using Tyr.Application.Extensions;
using Tyr.Domain.ClienteAggregate;
using Tyr.Domain.Interfaces;

namespace Tyr.Endpoints
{
    public static class ClienteEndpoint
    {
        public static void MapClienteEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Clientes", async (IReadRepository<Cliente> repository, CancellationToken cancellationToken) =>
            {
                var clientes = await repository.ListAsync(cancellationToken);

                return Results.Ok(clientes.ParseDTOList());
            });

            app.MapPost("/Clientes", async (ClienteInputDto clienteInputDto, IRepository<Cliente> repository, CancellationToken cancellationToken) =>
            {
                var cliente = new Cliente
                {
                    Nome = clienteInputDto.Nome,
                    Telefone = clienteInputDto.Telefone
                };

                await repository.AddAsync(cliente, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.Created($"/Clientes/{cliente.Id}", cliente.ParseDTO());
            });

            app.MapDelete("/Clientes/{id}", async (int id, IRepository<Cliente> repository, CancellationToken cancellationToken) =>
            {
                var buscarClienteId = await repository.GetByIdAsync(id, cancellationToken);
                if (buscarClienteId == null)
                {
                    return Results.NotFound();
                }

                await repository.DeleteAsync(buscarClienteId, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });

            app.MapGet("/Clientes/{id}", async (int id, IReadRepository<Cliente> repository, CancellationToken cancellationToken) =>
            {
                var buscarClienteId = await repository.GetByIdAsync(id, cancellationToken);
                if (buscarClienteId == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(buscarClienteId.ParseDTO());
            });

            app.MapPut("/Clientes/{id}", async (int id, IRepository<Cliente> repository, CancellationToken cancellationToken, ClienteInputDto clienteInputDto) =>
            {
                var buscarClienteId = await repository.GetByIdAsync(id, cancellationToken);
                if (buscarClienteId == null)
                {
                    return Results.NotFound();
                }

                buscarClienteId.Nome = clienteInputDto.Nome;
                buscarClienteId.Telefone = clienteInputDto.Telefone;

                await repository.UpdateAsync(buscarClienteId, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.Ok(buscarClienteId.ParseDTO());
            });
        }
    }
}
