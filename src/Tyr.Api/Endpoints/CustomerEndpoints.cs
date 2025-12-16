using Tyr.Application.DTOs;
using Tyr.Application.Extensions;
using Tyr.Domain.CustomerAggregate;
using Tyr.Domain.Interfaces;

namespace Tyr.Endpoints
{
    public static class CustomerEndpoint
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/customers", async (IReadRepository<Customer> repository, CancellationToken cancellationToken) =>
            {
                var customers = await repository.ListAsync(cancellationToken);

                return Results.Ok(customers.ParseDTOList());
            });

            app.MapPost("/customers", async (CustomerInputDto customerInputDto, IRepository<Customer> repository, CancellationToken cancellationToken) =>
            {
                var customer = new Customer
                {
                    Name = customerInputDto.Name,
                    Phone = customerInputDto.Phone
                };

                await repository.AddAsync(customer, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.Created($"/customers/{customer.Id}", customer.ParseDTO());
            });

            app.MapDelete("/customers/{id}", async (int id, IRepository<Customer> repository, CancellationToken cancellationToken) =>
            {
                var found = await repository.GetByIdAsync(id, cancellationToken);
                if (found == null)
                {
                    return Results.NotFound();
                }

                await repository.DeleteAsync(found, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            });

            app.MapGet("/customers/{id}", async (int id, IReadRepository<Customer> repository, CancellationToken cancellationToken) =>
            {
                var found = await repository.GetByIdAsync(id, cancellationToken);
                if (found == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(found.ParseDTO());
            });

            app.MapPut("/customers/{id}", async (int id, IRepository<Customer> repository, CancellationToken cancellationToken, CustomerInputDto customerInputDto) =>
            {
                var found = await repository.GetByIdAsync(id, cancellationToken);
                if (found == null)
                {
                    return Results.NotFound();
                }

                found.Name = customerInputDto.Name;
                found.Phone = customerInputDto.Phone;

                await repository.UpdateAsync(found, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                return Results.Ok(found.ParseDTO());
            });
        }
    }
}
