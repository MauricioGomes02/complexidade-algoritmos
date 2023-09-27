using System.Collections;

namespace ADTSet.Domain.Abstractions;

public abstract class Set
{
    public abstract ICollection Elements { get; }
    public abstract bool Contain(int element);
    public abstract void Insert(int element);
    public abstract void Remove(int element);
    public abstract Set Union(Set anotherSet);
    public abstract Set Intersect(Set anotherSet);
    public abstract Set Difference(Set anotherSet);
}
