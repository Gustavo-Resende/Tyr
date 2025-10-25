using Microsoft.EntityFrameworkCore;
using Tyr.Api.Extensions;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ProfissionalAggregate;
using Tyr.Domain.ProfissionalAggregate.Specifications;
using Tyr.Domain.ServicoAggregate;
using Tyr.DTOs;

namespace Tyr.Endpoints
{
    public static class ProfissionalEndpoints
    {
        public static void MapProfissionalEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/profissionais", async (IReadRepository<Profissional> repository, CancellationToken cancellationToken) =>
            {
                var spec = new ListProfissionaisComServicosSpec();
                var profissional = await repository.ListAsync(spec, cancellationToken);

                return Results.Ok(profissional.ParseDTOList());
            });

            app.MapPost("/profissionais", async (ProfissionalDto profissionalDto, IRepository<Profissional> repository, CancellationToken cancellationToken) =>
            {
                var profissional = new Profissional
                {
                    Nome = profissionalDto.Nome
                };

                await repository.AddAsync(profissional, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.Created($"/profissionais/{profissional.Id}", profissional.ParseDTO());
            });

            app.MapGet("/profissionais/{id}", async (int id, IReadRepository<Profissional> repository, CancellationToken cancellationToken) =>
            {
                var profissional = await repository.GetByIdAsync(id, cancellationToken);
                if (profissional is null)
                {
                    return Results.NotFound();

                }

                return Results.Ok(profissional.ParseDTO());
            });

            app.MapPut("/profissionais/{id}", async (int id, ProfissionalDto profissionalDto, IRepository<Profissional> repository, CancellationToken cancellationToken) =>
            {
                var profissionalExistente = await repository.GetByIdAsync(id, cancellationToken);
                if (profissionalExistente is null)
                {
                    return Results.NotFound();
                }

                profissionalExistente.Nome = profissionalDto.Nome;
                await repository.UpdateAsync(profissionalExistente, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });

            app.MapDelete("/profissionais/{id}", async (int id, IRepository<Profissional> repository, CancellationToken cancellationToken) =>
            {
                var profissionalExistente = await repository.GetByIdAsync(id, cancellationToken);
                if (profissionalExistente is null)
                {
                    return Results.NotFound();
                }

                await repository.DeleteAsync(profissionalExistente, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });

            // --- Endpoints de Sub-recursos ---

            app.MapPost("/profissionais/{profissionalId}/servicos", async (int id, ServicoDto servicoDto, IRepository<Profissional> repository, CancellationToken cancellationToken) =>
            {
                var profissionalExistente = await repository.GetByIdAsync(id, cancellationToken);
                if (profissionalExistente is null)
                {
                    return Results.NotFound();
                }

                var addServico = new Servico
                {
                    Nome = servicoDto.Nome,
                    Preco = servicoDto.Preco,
                    ProfissionalId = profissionalExistente.Id
                };

                await repository.AddAsync(profissionalExistente, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.Created($"/services/{addServico.Id}", addServico.ParseDTO());

            });
        }
    }
}
