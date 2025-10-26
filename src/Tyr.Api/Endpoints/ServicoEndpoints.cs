using Tyr.Api.Extensions;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ServicoAggregate;
using Tyr.DTOs;

namespace Tyr.Endpoints
{
    public static class ServicoEndpoints
    {
        public static void MapServicoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/servicos", async (IReadRepository<Servico> repository, CancellationToken cancellationToken) =>
            {
                var servicos = await repository.ListAsync(cancellationToken);
                return Results.Ok(servicos.ParseDTOList());
            });

            app.MapPost("/servicos", async (ServicoInputDto request, IRepository<Servico> repository, CancellationToken cancellationToken) =>
            {
                var profissionalExists = await repository.GetByIdAsync(request.ProfissionalId, cancellationToken);
                if (profissionalExists is null)
                {
                    return Results.NotFound();
                }

                var newServico = new Servico
                {
                    Nome = request.Nome,
                    Preco = request.Preco,
                    ProfissionalId = request.ProfissionalId
                };

                await repository.AddAsync(newServico, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);
                return Results.Created($"/servicos/{newServico.Id}", newServico.ParseDTO());
            });

            app.MapGet("/servicos/{id}", async (int id, IReadRepository<Servico> repository, CancellationToken cancellationToken) =>
            {
                var findServico = await repository.GetByIdAsync(id, cancellationToken);
                if (findServico is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(findServico.ParseDTO());
            });

            app.MapPut("/servicos/{id}", async (int id, ServicoInputDto servico, IRepository<Servico> repository, CancellationToken cancellationToken) =>
            {
                var findServico = await repository.GetByIdAsync(id, cancellationToken);
                if (findServico is null)
                {
                    return Results.NotFound();
                }

                findServico.Nome = servico.Nome;
                findServico.Preco = servico.Preco;

                await repository.UpdateAsync(findServico, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });

            app.MapDelete("/servicos/{id}", async (int id, IRepository<Servico> repository, CancellationToken cancellationToken) =>
            {
                var findServico = await repository.GetByIdAsync(id, cancellationToken);
                if (findServico is null)
                {
                    return Results.NotFound();
                }

                await repository.DeleteAsync(findServico, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });
        }
    }
}
