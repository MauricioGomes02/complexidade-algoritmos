namespace Binary.Tree.Domain.Structures;

public class Node
{
    internal Node(
        int key,
        Node? left = null,
        Node? right = null,
        Node? father = null)
    {
        Key = key;
        Left = left;
        Right = right;
        Father = father;
    }

    public int Key { get; private set; }
    public Node? Left { get; private set; }
    public Node? Right { get; private set; }
    public Node? Father { get; private set; }

    internal void AddLeft(int key, Node? father)
    {
        Left = new Node(key, father: father);
    }

    internal void AddLeft(Node node)
    {
        Left = node;
    }

    internal void AddRight(int key, Node? father)
    {
        Right = new Node(key, father: father);
    }

    internal void AddRight(Node node)
    {
        Right = node;
    }

    internal void AddFather(Node? father)
    {
        Father = father;
    }
}
