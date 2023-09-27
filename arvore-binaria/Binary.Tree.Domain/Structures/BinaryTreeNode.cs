namespace Binary.Tree.Domain.Structures;

public class BinaryTreeNode
{
    public BinaryTreeNode(int key)
    {
        Key = key;
    }

    public virtual int Key { get; set; }
    public virtual BinaryTreeNode? Left { get; set; }
    public virtual BinaryTreeNode? Right { get; set; }
    public virtual BinaryTreeNode? Father { get; set; }
    public virtual BinaryTree? Tree { get; set; }
}
