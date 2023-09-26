using Binary.Tree.Domain.Structures;

namespace Binary.Tree.Domain;

public class BinaryTree
{
    public BinaryTree(IEnumerable<int> keys = null)
    {
        if (keys is null || !keys.Any())
        {
            return;
        }

        foreach (var key in keys)
        {
            Insert(key);
        }
    }

    public Node? Root { get; private set; }

    public Node? Search(int key)
    {
        return Search(key, Root);
    }


    public void Insert(int key)
    {
        var currentNode = Root;
        var auxiliarNode = null as Node;

        while (currentNode is not null)
        {
            auxiliarNode = currentNode;
            currentNode = key < currentNode.Key ? currentNode.Left : currentNode.Right;
        }

        if (auxiliarNode is null)
        {
            Root = new Node(key);
            return;
        }

        if (key < auxiliarNode.Key)
        {
            auxiliarNode.AddLeft(key, auxiliarNode);
            return;
        }

        auxiliarNode.AddRight(key, auxiliarNode);
    }

    public void Delete(Node node)
    {
        if (node.Left is null)
        {
            Transplant(node, node.Right!);
            return;
        }

        if (node.Right is null)
        {
            Transplant(node, node.Left!);
            return;
        }
        
        var minimumNode = Minimum(node.Right);
        if (!ReferenceEquals(minimumNode?.Father, node))
        {
            Transplant(minimumNode!, minimumNode!.Right!);
            minimumNode.AddRight(node.Right!);
            minimumNode.Right!.AddFather(minimumNode);
        }
        Transplant(node, minimumNode!);
        minimumNode!.AddLeft(node.Left);
        minimumNode.Left!.AddFather(minimumNode);
    }

    private Node? Search(int key, Node? node)
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

    private void Transplant(Node original, Node @new)
    {
        if (original.Father is null)
        {
            Root = @new;
            return;
        }

        if (ReferenceEquals(original, original.Father.Left))
        {
            original.Father.AddLeft(@new);
        }
        else
        {
            original.Father.AddRight(@new);
        }

        @new?.AddFather(original.Father);
    }

    private static Node? Minimum(Node? node)
    {
        while (node?.Left is not null)
        {
            node = node.Left;
        }

        return node;
    }
}
