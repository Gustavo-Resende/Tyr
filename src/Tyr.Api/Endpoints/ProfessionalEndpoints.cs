using Tyr.Application.DTOs;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ProfessionalAggregate;
using Tyr.Domain.ProfessionalAggregate.Specifications;
using Tyr.Application.Extensions;

namespace Tyr.Endpoints
{
    public static class ProfessionalEndpoints
    {
        public static void MapProfessionalEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/professionals", async (IReadRepository<Professional> repository, CancellationToken cancellationToken) =>
            {
                var spec = new ListProfessionalsWithServicesSpec();
                var professionals = await repository.ListAsync(spec, cancellationToken);

                return Results.Ok(professionals.ParseDTOList());
            });

            app.MapPost("/professionals", async (ProfessionalDto professionalDto, IRepository<Professional> repository, CancellationToken cancellationToken) =>
            {
                var professional = new Professional
                {
                    Name = professionalDto.Name
                };

                await repository.AddAsync(professional, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.Created($"/professionals/{professional.Id}", professional.ParseDTO());
            });

            app.MapGet("/professionals/{id}", async (int id, IReadRepository<Professional> repository, CancellationToken cancellationToken) =>
            {
                var professional = await repository.GetByIdAsync(id, cancellationToken);
                if (professional is null)
                {
                    return Results.NotFound();

                }

                return Results.Ok(professional.ParseDTO());
            });

            app.MapPut("/professionals/{id}", async (int id, ProfessionalDto professionalDto, IRepository<Professional> repository, CancellationToken cancellationToken) =>
            {
                var existing = await repository.GetByIdAsync(id, cancellationToken);
                if (existing is null)
                {
                    return Results.NotFound();
                }

                existing.Name = professionalDto.Name;
                await repository.UpdateAsync(existing, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });

            app.MapDelete("/professionals/{id}", async (int id, IRepository<Professional> repository, CancellationToken cancellationToken) =>
            {
                var existing = await repository.GetByIdAsync(id, cancellationToken);
                if (existing is null)
                {
                    return Results.NotFound();
                }

                await repository.DeleteAsync(existing, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });

        }
    }
}
