using Tyr.Application.Services.Dtos;
using Tyr.Application.Services.Extensions;
using Tyr.Domain.Interfaces;
using Tyr.Domain.ServiceAggregate;

namespace Tyr.Endpoints
{
    public static class ServiceEndpoints
    {
        public static void MapServiceEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/services", async (IReadRepository<Service> repository, CancellationToken cancellationToken) =>
            {
                var services = await repository.ListAsync(cancellationToken);
                return Results.Ok(services.ParseDTOList());
            });

            app.MapPost("/services", async (ServiceDto request, IRepository<Service> repository, CancellationToken cancellationToken) =>
            {
                var professionalExists = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (professionalExists is null)
                {
                    return Results.NotFound();
                }

                var newService = new Service(request.Name, request.Price, request.Duration);

                await repository.AddAsync(newService, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);
                return Results.Created($"/services/{newService.Id}", newService.ParseDTO());
            });

            app.MapGet("/services/{id}", async (int id, IReadRepository<Service> repository, CancellationToken cancellationToken) =>
            {
                var findService = await repository.GetByIdAsync(id, cancellationToken);
                if (findService is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(findService.ParseDTO());
            });

            app.MapPut("/services/{id}", async (int id, ServiceDto servico, IRepository<Service> repository, CancellationToken cancellationToken) =>
            {
                var findService = await repository.GetByIdAsync(id, cancellationToken);
                if (findService is null)
                {
                    return Results.NotFound();
                }

                findService.Name = servico.Name;
                findService.Price = servico.Price;

                await repository.UpdateAsync(findService, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });

            app.MapDelete("/services/{id}", async (int id, IRepository<Service> repository, CancellationToken cancellationToken) =>
            {
                var findService = await repository.GetByIdAsync(id, cancellationToken);
                if (findService is null)
                {
                    return Results.NotFound();
                }

                await repository.DeleteAsync(findService, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });
        }
    }
}
