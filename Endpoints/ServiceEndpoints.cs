using Microsoft.EntityFrameworkCore;
using Tyr.Data;
using Tyr.Models;

namespace Tyr.Endpoints
{
    public static class ServiceEndpoints
    {
        public static void MapServiceEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/services", async (AppDbContext context) =>
            {
                var services = await context.Services.ToListAsync();
                return Results.Ok(services);
            });

            app.MapGet("/services/{id}", async (int id, AppDbContext context) =>
            {
                var findservice = await context.Services.FirstOrDefaultAsync(x => x.Id == id);
                if (findservice is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }
                return Results.Ok(findservice);
            });

            app.MapPut("/services/{id}", async (int id, Service servico, AppDbContext context) =>
            {
                var findservice = await context.Services.FirstOrDefaultAsync(x => x.Id == id);

                if (findservice is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }

                findservice.Name = servico.Name;
                findservice.Price = servico.Price;
                await context.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapDelete("/services/{id}", async (int id, AppDbContext context) =>
            {
                var findservice = await context.Services.FirstOrDefaultAsync(x => x.Id == id);

                if (findservice is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }

                context.Remove(findservice);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
