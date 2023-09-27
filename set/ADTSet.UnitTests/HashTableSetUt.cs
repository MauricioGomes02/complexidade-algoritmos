using ADTSet.Domain;

namespace ADTSet.UnitTests;

public class HashTableSetUt
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var sut = new HashTableSet();
        var other = new HashTableSet();

        // Act
        sut.Insert(1);
        sut.Insert(2);
        sut.Insert(3);

        other.Insert(4);
        other.Insert(5);

        var newSet = sut.Union(other);
    }
}