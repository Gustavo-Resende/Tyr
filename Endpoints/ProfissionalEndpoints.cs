using Microsoft.EntityFrameworkCore;
using Tyr.Data;
using Tyr.DTOs;
using Tyr.Domain.Entidades;
using Tyr.Models;

namespace Tyr.Endpoints
{
    public static class ProfissionalEndpoints
    {
        public static void MapProfissionalEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/profissionais", async (AppDbContext context) =>
            {
                var profissionais = await context.Profissionais.ToListAsync();
                return Results.Ok(profissionais);
            });

            app.MapPost("/profissionais", async (ProfissionalDto profissionalDto, AppDbContext context) =>
            {
                var profissional = new Profissional
                {
                    Nome = profissionalDto.Nome,
                };

                context.Profissionais.Add(profissional);
                await context.SaveChangesAsync();
                return Results.Created($"/profissionais/{profissional.Id}", profissional);
            });

            app.MapGet("/profissionais/{id}", async (int id, AppDbContext context) =>
            {
                var profissional = await context.Profissionais.FirstOrDefaultAsync(x => x.Id == id);
                if (profissional is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }
                return Results.Ok(profissional);
            });

            app.MapPut("/profissionais/{id}", async (int id, Profissional profissional, AppDbContext context) =>
            {
                var profissionalExistente = await context.Profissionais.FirstOrDefaultAsync(x => x.Id == id);

                if (profissionalExistente is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }

                profissionalExistente.Nome = profissional.Nome;
                await context.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapDelete("/profissionais/{id}", async (int id, AppDbContext context) =>
            {
                var profissional = await context.Profissionais.FirstOrDefaultAsync(x => x.Id == id);

                if (profissional is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }

                context.Remove(profissional);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });

            // --- Endpoints de Sub-recursos ---

            app.MapGet("/profissionais/{profissionalId}/servicos", async (int profissionalId, AppDbContext context) =>
            {
                var entidadeProfissional = await context.Profissionais.Where(p => p.Id
                == profissionalId).Include(p => p.Servicos).FirstOrDefaultAsync(p => p.Id == profissionalId);

                var profissionalDto = new ProfissionalDto(entidadeProfissional.Id, entidadeProfissional.Nome, entidadeProfissional.Servicos.Select(s => new ServicoDto(s.Id, s.Nome, s.Preco)).ToList());

                return Results.Ok(profissionalDto);
            });

            app.MapPost("/profissionais/{profissionalId}/servicos", async (int profissionalId, ServicoDto servicoDto, AppDbContext context) =>
            {
                var entidadeProfissional = await context.Profissionais.Where(p => p.Id
                == profissionalId).Include(p => p.Servicos).FirstOrDefaultAsync(p => p.Id == profissionalId);

                var addServico = new Servico
                {
                    Nome = servicoDto.Nome,
                    Preco = servicoDto.Preco,
                    ProfissionalId = entidadeProfissional.Id
                };

                context.Servicos.Add(addServico);
                await context.SaveChangesAsync();
                var responseDto = new ServicoDto(addServico.Id, addServico.Nome, addServico.Preco);


                return Results.Created($"/services/{addServico.Id}", responseDto);

            });
        }
    }
}
