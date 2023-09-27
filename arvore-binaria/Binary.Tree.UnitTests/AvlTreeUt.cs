using Binary.Tree.Domain;

namespace Binary.Tree.UnitTests;

public class AvlTreeUt
{
    [Fact]
    public void WhenInsertingKeysInArithmeticProgression()
    {
        var sut = new AvlTree();

        sut.Insert(1);
        sut.Insert(2);
        sut.Insert(3);
        sut.Insert(4);
        sut.Insert(5);
        sut.Insert(6);

        Assert.NotNull(sut.Root);
        Assert.Equal(4, sut.Root.Key);
        Assert.NotNull(sut.Root.Left);
        Assert.Equal(2, sut.Root.Left.Key);
        Assert.NotNull(sut.Root.Left.Left);
        Assert.Equal(1, sut.Root.Left.Left.Key);
        Assert.NotNull(sut.Root.Left.Right);
        Assert.Equal(3, sut.Root.Left.Right.Key);
        Assert.NotNull(sut.Root.Right);
        Assert.Equal(5, sut.Root.Right.Key);
        Assert.NotNull(sut.Root.Right.Right);
        Assert.Equal(6, sut.Root.Right.Right.Key);
    }

    [Fact]
    public void WhenInsertingKeysInDescendingArithmeticProgression()
    {
        var sut = new AvlTree();

        sut.Insert(6);
        sut.Insert(5);
        sut.Insert(4);
        sut.Insert(3);
        sut.Insert(2);
        sut.Insert(1);

        Assert.NotNull(sut.Root);
        Assert.Equal(3, sut.Root.Key);
        Assert.NotNull(sut.Root.Left);
        Assert.Equal(2, sut.Root.Left.Key);
        Assert.NotNull(sut.Root.Left.Left);
        Assert.Equal(1, sut.Root.Left.Left.Key);
        Assert.NotNull(sut.Root.Right);
        Assert.Equal(5, sut.Root.Right.Key);
        Assert.NotNull(sut.Root.Right.Left);
        Assert.Equal(4, sut.Root.Right.Left.Key);
        Assert.NotNull(sut.Root.Right.Right);
        Assert.Equal(6, sut.Root.Right.Right.Key);
    }

    [Fact]
    public void WhenInsertingKeysInZigZag()
    {
        var sut = new AvlTree();

        sut.Insert(1);
        sut.Insert(3);
        sut.Insert(2);
        sut.Insert(4);
        sut.Insert(6);
        sut.Insert(5);

        Assert.NotNull(sut.Root);
        Assert.Equal(4, sut.Root.Key);
        Assert.NotNull(sut.Root.Left);
        Assert.Equal(2, sut.Root.Left.Key);
        Assert.NotNull(sut.Root.Left.Left);
        Assert.Equal(1, sut.Root.Left.Left.Key);
        Assert.NotNull(sut.Root.Left.Right);
        Assert.Equal(3, sut.Root.Left.Right.Key);
        Assert.NotNull(sut.Root.Right);
        Assert.Equal(5, sut.Root.Right.Key);
        Assert.NotNull(sut.Root.Right.Right);
        Assert.Equal(6, sut.Root.Right.Right.Key);
    }

    [Fact]
    public void WhenInsertingKeysInDescendingZigZag()
    {
        var sut = new AvlTree();

        sut.Insert(3);
        sut.Insert(1);
        sut.Insert(2);
        sut.Insert(6);
        sut.Insert(4);
        sut.Insert(5);

        Assert.NotNull(sut.Root);
        Assert.Equal(4, sut.Root.Key);
        Assert.NotNull(sut.Root.Left);
        Assert.Equal(2, sut.Root.Left.Key);
        Assert.NotNull(sut.Root.Left.Left);
        Assert.Equal(1, sut.Root.Left.Left.Key);
        Assert.NotNull(sut.Root.Left.Right);
        Assert.Equal(3, sut.Root.Left.Right.Key);
        Assert.NotNull(sut.Root.Right);
        Assert.Equal(6, sut.Root.Right.Key);
        Assert.NotNull(sut.Root.Right.Left);
        Assert.Equal(5, sut.Root.Right.Left.Key);
    }
}
