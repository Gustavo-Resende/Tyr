using Microsoft.EntityFrameworkCore;
using Tyr.Data;
using Tyr.DTOs;
using Tyr.Models;

namespace Tyr.Endpoints
{
    public static class ClienteEndpoint
    {
        public static void MapClienteEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Clientes", async (AppDbContext context) =>
            {
                var clientes = await context.Clientes.Select(c => new ClienteOutputDto(c.Id, c.Nome, c.Telefone)).ToListAsync();

                return Results.Ok(clientes);
            });

            app.MapPost("/Clientes", async (ClienteInputDto clienteInputDto, AppDbContext context) =>
            {
                var cliente = new Cliente
                {
                    Nome = clienteInputDto.Nome,
                    Telefone = clienteInputDto.Telefone
                };

                context.Clientes.Add(cliente);
                await context.SaveChangesAsync();

                var clienteOutput = new ClienteOutputDto(cliente.Id, cliente.Nome, cliente.Telefone);

                return Results.Created($"/Clientes/{cliente.Id}", clienteOutput);

            });

            app.MapDelete("/Clientes/{id}", async (int id, AppDbContext context) =>
            {
                var buscarClienteId = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
                if (buscarClienteId == null)
                {
                    return Results.NotFound(buscarClienteId); 
                }

                context.Clientes.Remove(buscarClienteId);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapGet("/Clientes/{id}", async (int id, AppDbContext context) =>
            {
                var buscarClienteId = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
                if (buscarClienteId == null)
                {
                    return Results.NotFound(buscarClienteId);
                }

                var clienteOutput = new ClienteOutputDto(buscarClienteId.Id, buscarClienteId.Nome, buscarClienteId.Telefone);
                return Results.Ok(clienteOutput);
            });

            app.MapPut("/Clientes/{id}", async (int id, ClienteInputDto clienteInputDto, AppDbContext context) =>
            {
                var buscarClienteId = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
                if (buscarClienteId == null)
                {
                    return Results.NotFound(buscarClienteId);
                }

                buscarClienteId.Nome = clienteInputDto.Nome;
                buscarClienteId.Telefone = clienteInputDto.Telefone;

                await context.SaveChangesAsync();

                var clienteOutput = new ClienteOutputDto(buscarClienteId.Id, buscarClienteId.Nome, buscarClienteId.Telefone);
                return Results.Ok(clienteOutput);
            });
        }
    }
}
