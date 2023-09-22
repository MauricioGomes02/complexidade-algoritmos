using Binary.Tree.Domain.Structures;

namespace Binary.Tree.Domain;

public class BinaryTree
{
    public BinaryTree(Node root)
    {
        Root = root;
    }

    public Node Root { get; private set; }
}
