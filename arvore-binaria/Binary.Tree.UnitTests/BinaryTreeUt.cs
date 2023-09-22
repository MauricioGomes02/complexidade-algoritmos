using Binary.Tree.Domain;
using Binary.Tree.Domain.Exceptions;
using Binary.Tree.Domain.Structures;

namespace Binary.Tree.UnitTests;

public class BinaryTreeUt
{

    [Fact]
    public void WhenCreate_ShouldLeaveRootElementAccessible()
    {
        // Arrange
        var keys = new List<int> { default };

        // Act
        var sut = new BinaryTree(keys);

        // Assert
        Assert.Equal(default, sut.Root.Key);
    }

    [Fact]
    public void WhenCreate_ShouldFollowParentAndChildNodeRule()
    {
        // Arrange
        var keys = new List<int> { 2, 3, 1 };
        var sut = new BinaryTree(keys);

        // Act
        var node = sut.Search(2)!;

        // Assert
        Assert.Equal(2, node.Key);
        Assert.Null(node.Father);
        Assert.Null(node.Left);
        Assert.NotNull(node.Right);
        Assert.Equal(3, node.Right.Key);

        Assert.NotNull(node.Right.Father);
        Assert.Equal(2, node.Right.Father.Key);
        Assert.NotNull(node.Right.Left);
        Assert.Equal(1, node.Right.Left.Key);
        Assert.Null(node.Right.Right);

        Assert.NotNull(node.Right.Left.Father);
        Assert.Equal(3, node.Right.Left.Father.Key);
        Assert.Null(node.Right.Left.Left);
        Assert.Null(node.Right.Left.Right);
    }
}
