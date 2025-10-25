namespace Tyr.Domain;

/// <summary>
/// Classe base para todas as entidades de Domínio.
/// </summary>
/// <typeparam name="TId">O tipo do identificador (ex: int, Guid).</typeparam>
public abstract class EntityBase<TId> : IEquatable<EntityBase<TId>>
    where TId : struct, IEquatable<TId>
{
    public TId Id { get; protected set; }

    public bool IsTransient() => Id.Equals(default);

    protected EntityBase() { }

    public override bool Equals(object? obj) => obj is EntityBase<TId> other && Equals(other);

    public bool Equals(EntityBase<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        if (IsTransient() || other.IsTransient())
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        // Se for novo, usa o hashcode base. Senão, usa o hashcode da ID.
        if (IsTransient()) return base.GetHashCode();
        return Id.GetHashCode(); // Versão simplificada para não usar variáveis privadas
    }

    // Sobrecarga de operadores
    public static bool operator ==(EntityBase<TId>? left, EntityBase<TId>? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(EntityBase<TId>? left, EntityBase<TId>? right) =>
        !(left == right);
}