namespace Binary.Tree.Domain.Structures;

public class Node
{
    public Node(
        int key,
        Node? father = null,
        Node? left = null,
        Node? right = null)
    {
        Key = key;
        Father = father;
        Left = left;
        Right = right;
    }

    public int Key { get; private set; }
    public Node? Father { get; private set; }
    public Node? Left { get; private set; }
    public Node? Right { get; private set; }
}
