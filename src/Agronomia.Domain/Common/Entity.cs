namespace Agronomia.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected init; }

    protected Entity()
    {
    }

    protected Entity(Guid id)
    {
        Id = id;
    }

    protected bool Equals(Entity other)
        => Id.Equals(other.Id);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Entity)obj);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right)
        => Equals(left, right);

    public static bool operator !=(Entity? left, Entity? right)
        => !Equals(left, right);
}
