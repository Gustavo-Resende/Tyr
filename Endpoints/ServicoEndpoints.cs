using Microsoft.EntityFrameworkCore;
using Tyr.Data;
using Tyr.Models;

namespace Tyr.Endpoints
{
    public static class ServicoEndpoints
    {
        public static void MapServicoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/servicos", async (AppDbContext context) =>
            {
                var servicos = await context.Servicos.ToListAsync();
                return Results.Ok(servicos);
            });

            app.MapGet("/servicos/{id}", async (int id, AppDbContext context) =>
            {
                var findServico = await context.Servicos.FirstOrDefaultAsync(x => x.Id == id);
                if (findServico is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }
                return Results.Ok(findServico);
            });

            app.MapPut("/servicos/{id}", async (int id, Servico servico, AppDbContext context) =>
            {
                var findServico = await context.Servicos.FirstOrDefaultAsync(x => x.Id == id);

                if (findServico is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }

                findServico.Nome = servico.Nome;
                findServico.Preco = servico.Preco;
                await context.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapDelete("/servicos/{id}", async (int id, AppDbContext context) =>
            {
                var findServico = await context.Servicos.FirstOrDefaultAsync(x => x.Id == id);

                if (findServico is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }

                context.Remove(findServico);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
