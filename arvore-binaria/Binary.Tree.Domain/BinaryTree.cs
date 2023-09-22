using Binary.Tree.Domain.Exceptions;
using Binary.Tree.Domain.Structures;

namespace Binary.Tree.Domain;

public class BinaryTree
{
    public BinaryTree(IEnumerable<int> keys)
    {
        if (keys is null || !keys.Any())
        {
            throw new ArgumentException("Must provide at least one key");
        }

        var firstKey = keys.First();
        Root = new Node(firstKey);
        var currentNode = Root;

        foreach (var key in keys.Skip(1)) 
        {
            if (key <= currentNode!.Key)
            {
                currentNode.AddLeft(key, currentNode);
                currentNode = currentNode.Left;
                continue;
            }

            if (key >= currentNode!.Key)
            {
                currentNode.AddRight(key, currentNode);
                currentNode = currentNode.Right;
            }
        }
    }

    public Node Root { get; private set; }

    public Node? Search(int key)
    {
        return Search(key, Root);
    }

    private Node? Search(int key, Node node)
    {
        if (node is null || node.Key == key)
        {
            return node;
        }

        if (key < node.Key)
        {
            return Search(key, node.Left!);
        }

        return Search(key, node.Right!);
    }
}
