using Binary.Tree.Domain;

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
        Assert.Equal(default, sut.Root!.Key);
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
        Assert.NotNull(node.Left);
        Assert.NotNull(node.Right);

        Assert.Equal(1, node.Left.Key);
        Assert.NotNull(node.Left.Father);
        Assert.Equal(2, node.Left.Father.Key);

        Assert.Equal(3, node.Right.Key);
        Assert.NotNull(node.Right.Father);
        Assert.Equal(2, node.Right.Father.Key);
    }

    [Fact]
    public void WhenInsertInAnEmptyTree_ShouldHaveTheRootNodeWithTheEnteredValue()
    {
        // Arrange
        var sut = new BinaryTree();

        // Act
        sut.Insert(2);

        // Assert
        var node = sut.Root;

        Assert.NotNull(node);
        Assert.Equal(2, node.Key);
        Assert.Null(node.Father);
        Assert.Null(node.Left);
        Assert.Null(node.Right);
    }

    [Fact]
    public void WhenInsertASmallerKey_ShouldInsertOnTheLeft()
    {
        // Arrange
        var keys = new List<int> { 2 };
        var sut = new BinaryTree(keys);

        // Act
        sut.Insert(1);

        // Assert
        var node = sut.Root!;

        Assert.NotNull(node.Left);
        Assert.Null(node.Right);

        Assert.NotNull(node.Left.Father);
        Assert.Equal(1, node.Left.Key);
    }

    [Fact]
    public void WhenInsertALargerKey_ShouldInsertOnTheLeft()
    {
        // Arrange
        var keys = new List<int> { 2 };
        var sut = new BinaryTree(keys);

        // Act
        sut.Insert(3);

        // Assert
        var node = sut.Root!;

        Assert.NotNull(node.Right);
        Assert.Null(node.Left);

        Assert.NotNull(node.Right.Father);
        Assert.Equal(3, node.Right.Key);
    }

    [Fact]
    public void WhenDeleteAndTheLeftIsNull_ShouldReplaceTheFatherNodeWithTheChildNodeOnTheRight()
    {
        // Arrange
        var keys = new List<int> { 2, 3 };
        var sut = new BinaryTree(keys);

        // Act
        sut.Delete(sut.Root!);

        // Assert
        Assert.NotNull(sut.Root);
        Assert.Equal(3, sut.Root.Key);
    }

    [Fact]
    public void WhenDeleteAndTheRightIsNull_ShouldReplaceTheFatherNodeWithTheChildNodeOnTheLeft()
    {
        // Arrange
        var keys = new List<int> { 2, 1 };
        var sut = new BinaryTree(keys);

        // Act
        sut.Delete(sut.Root!);

        // Assert
        Assert.NotNull(sut.Root);
        Assert.Equal(1, sut.Root.Key);
    }
}
