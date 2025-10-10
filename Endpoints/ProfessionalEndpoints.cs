using Microsoft.EntityFrameworkCore;
using Tyr.Data;
using Tyr.DTOs;
using Tyr.Models;

namespace Tyr.Endpoints
{
    public static class ProfessionalEndpoints
    {
        public static void MapProfessionalEndpoints (this IEndpointRouteBuilder app)
        {
            app.MapGet("/professionals", async (AppDbContext context) =>
            {
                var professionals = await context.Professionals.ToListAsync();
                return Results.Ok(professionals);
            });

            app.MapPost("/professionals", async (ProfessionalDto professionalDto, AppDbContext context) =>
            {
                var professional = new Professional
                {
                    Name = professionalDto.Name,
                    Specialty = professionalDto.Specialty
                };

                context.Professionals.Add(professional);
                await context.SaveChangesAsync();
                return Results.Created($"/professionals/{professional.Id}", professional);
            });

            app.MapGet("/professionals/{id}", async (int id, AppDbContext context) =>
            {
                var professional = await context.Professionals.FirstOrDefaultAsync(x => x.Id == id);
                if (professional is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }
                return Results.Ok(professional);
            });

            app.MapPut("/professionals/{id}", async (int id, Professional profissional, AppDbContext context) =>
            {
                var professional = await context.Professionals.FirstOrDefaultAsync(x => x.Id == id);

                if (professional is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }

                professional.Name = profissional.Name;
                professional.Specialty = profissional.Specialty;
                await context.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapDelete("/professionals/{id}", async (int id, AppDbContext context) =>
            {
                var professional = await context.Professionals.FirstOrDefaultAsync(x => x.Id == id);

                if (professional is null)
                {
                    return Results.NotFound($"ID {id} não encontrado!");
                }

                context.Remove(professional);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });

            // --- Endpoints de Sub-recursos ---

            app.MapGet("/professionals/{professionalId}/services", async (int professionalId, AppDbContext context) =>
            {
                var entidadeProfissional = await context.Professionals.Where(p => p.Id
                == professionalId).Include(p => p.Services).FirstOrDefaultAsync(p => p.Id == professionalId);

                var profissionalDto = new ProfessionalDto(entidadeProfissional.Id, entidadeProfissional.Name, entidadeProfissional.Specialty,
                    entidadeProfissional.Services.Select(s => new ServiceDto(s.Id, s.Name, s.Price)).ToList());

                return Results.Ok(profissionalDto);
            });

            app.MapPost("/professionals/{professionalId}/services", async (int professionalId, ServiceDto servicoDto, AppDbContext context) =>
            {
                var entidadeProfissional = await context.Professionals.Where(p => p.Id
                == professionalId).Include(p => p.Services).FirstOrDefaultAsync(p => p.Id == professionalId);

                var addServico = new Service
                {
                    Name = servicoDto.Name,
                    Price = servicoDto.Price,
                    ProfessionalId = entidadeProfissional.Id
                };

                context.Services.Add(addServico);
                await context.SaveChangesAsync();
                var responseDto = new ServiceDto(addServico.Id, addServico.Name, addServico.Price);


                return Results.Created($"/services/{addServico.Id}", responseDto);

            });
        }
    }
}
