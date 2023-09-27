using ADTSet.Domain.Abstractions;
using System.Collections;

namespace ADTSet.Domain;

public class HashTableSet : Set
{
    private readonly Hashtable _hashTable;

    public HashTableSet()
    {
        _hashTable = new Hashtable();
    }

    public override ICollection Elements => _hashTable.Keys;

    public override bool Contain(int element)
    {
        // Time: O(1); Space: O(n)
        return _hashTable.Contains(element);
    }

    public override Set Difference(Set anotherSet)
    {
        var newSet = new HashTableSet();
        var keys = _hashTable.Keys;

        // Time: O(n); Space: O(n)
        foreach (int key in keys)
        {
            // O(1)
            if (!anotherSet.Contain(key))
            {
                // O(1)
                newSet.Insert(key);
            }
        }

        // Time: O(n) * O(1); Space: O(n) -> Time: O(n); Space: O(n)
        return newSet;
    }

    public override void Insert(int element)
    {
        // Does not allow duplicate keys
        // Time: O(1); Space: O(n)
        _hashTable.Add(element, null);
    }

    public override Set Intersect(Set anotherSet)
    {
        var newSet = new HashTableSet();
        var keys = _hashTable.Keys;

#pragma warning disable S3267 // Loops should be simplified with "LINQ" expressions

        // Time: O(n); Space: O(n)
        foreach (int key in keys)
        {
            if (anotherSet.Contain(key))
            {
                // O(1)
                newSet.Insert(key);
            }
        }

#pragma warning restore S3267 // Loops should be simplified with "LINQ" expressions

        // Time: O(n) * O(1); Space: O(n) -> Time: O(n); Space: O(n)
        return newSet;
    }

    public override void Remove(int element)
    {
        // Time: O(1); Space: O(n)
        _hashTable.Remove(element);
    }

    public override Set Union(Set anotherSet)
    {
        var newSet = new HashTableSet();
        var keys = _hashTable.Keys;

        // Time: O(n); Space: O(n)
        foreach (int key in keys)
        {
            // O(1)
            newSet.Insert(key);
        }

        var anotherSetKeys = anotherSet.Elements;

        // Time: O(m); Space: O(m)
        foreach (int key in anotherSetKeys)
        {
            if (!newSet.Contain(key))
            {
                newSet.Insert(key);
            }
        }

        // Time: O(n) + O(m); Space: O(n) + O(m)
        return newSet;
    }
}
