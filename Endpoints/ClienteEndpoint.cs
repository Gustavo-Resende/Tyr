using Microsoft.EntityFrameworkCore;
using Tyr.Data;
using Tyr.DTOs;
using Tyr.Models;

namespace Tyr.Endpoints
{
    public static class ClienteEndpoint
    {
        public static void MapClienteEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Clientes", async (AppDbContext context) =>
            {
                var cliente = await context.Clientes.ToListAsync();
                return Results.Ok(cliente);
            });

            app.MapPost("/Clientes", async (ClienteInputDto clienteDto, AppDbContext context) =>
            {
                var cliente = new Cliente
                {
                    Id = clienteDto.Id,
                    Nome = clienteDto.Name,
                    Telefone = clienteDto.Telefone
                };

                context.Clientes.Add(cliente);
                await context.SaveChangesAsync();

                var createDto = new ClienteInputDto(cliente.Id, cliente.Nome, cliente.Telefone);

                return Results.Created($"/Clientes/{cliente.Id}", clienteDto);

            });

        }
    }
}
