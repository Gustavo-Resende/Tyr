using Ardalis.Specification.EntityFrameworkCore;
using Tyr.Domain.Entities;
using Tyr.Domain.Interfaces;
using Tyr.Infrastructure.Persistence;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
    public EfRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}