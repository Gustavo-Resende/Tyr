using Ardalis.Specification;
using Tyr.Domain.Entities;

namespace Tyr.Domain.Interfaces;

/// <summary>
/// Uma abstração para persistência, baseada em Specification.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{ }

/// <summary>
/// Uma abstração para operações de persistência somente leitura, baseada em BNB.Specification.
/// Use isso principalmente para buscar entidades de domínio rastreáveis, não para consultas personalizadas.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{ }
