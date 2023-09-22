using Binary.Tree.Domain;
using Binary.Tree.Domain.Structures;

namespace Binary.Tree.UnitTests;

public class BinaryTreeUt
{
    public BinaryTreeUt()
    {
    }

    [Fact]
    public void WhenCreate_ShouldLeaveRootElementAccessible()
    {
        // Arrange
        var node = new Node(default);

        // Act
        var sut = new BinaryTree(node);

        // Assert
        Assert.Same(sut.Root, node);
    }
}
