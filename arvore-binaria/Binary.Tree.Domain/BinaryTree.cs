using Binary.Tree.Domain.Exceptions;
using Binary.Tree.Domain.Structures;

namespace Binary.Tree.Domain;

public class BinaryTree
{
    public BinaryTree()
    {
        Root = null;
    }

    public BinaryTreeNode? Root { get;  set; }

    public BinaryTreeNode? Search(int key)
    {
        var node = Root;
        while (node is not null)
        {
            if (node.Key == key)
            {
                return node;
            }

            if (key < node.Key)
            {
                node = node.Left;
            }
            else
            {
                node = node.Right;
            }
        }

        return null;
    }

    public virtual void Insert(int key)
    {
        var node = new BinaryTreeNode(key);
        Insert(node);
    }

    public virtual void Insert(BinaryTreeNode node)
    {
        if (Root is null)
        {
            Root = node;
            node.Tree = this;
            return;
        }

        node.Father ??= Root;

        if (node.Key == node.Father.Key)
        {
            throw new DuplicateKeyException();
        }

        // Left
        if (node.Key < node.Father.Key)
        {
            if (node.Father.Left is null)
            {
                node.Father.Left = node;
                node.Tree = this;
            }
            else
            {
                node.Father = node.Father.Left;
                Insert(node);
            }
        }
        // Right
        else
        {
            if (node.Father.Right is null)
            {
                node.Father.Right = node;
                node.Tree = this;
            }
            else
            {
                node.Father = node.Father.Right;
                Insert(node);
            }
        }
    }

    public virtual bool Delete(int key)
    {
        var node = Search(key);
        return Delete(node);
    }

    public virtual bool Delete(BinaryTreeNode? node)
    {
        if (node is null)
        {
            return false;
        }

        // Root
        if (ReferenceEquals(Root, node) && node.Left is null && node.Right is null)
        {
            Root = null;
            node.Tree = null;
            return true;
        }

        // No Children
        if (node.Left is null && node.Right is null)
        {
            var father = node.Father!;
            if (IsLeftChild(node))
            {
                father.Left = null;
            }
            else
            {
                father.Right = null;
            }

            node.Tree = null;
            node.Father = null;
        }
        // One Children
        else if (node.Left is not null && node.Right is null)
        {
            var father = node.Father!;
            node.Left.Father = father;

            if (ReferenceEquals(Root, node))
            {
                Root = node.Left;
            }

            if (IsLeftChild(node))
            {
                father.Left = node.Left;
            }
            else
            {
                father.Right = node.Left;
            }
        }
        else if (node.Right is not null && node.Left is null)
        {
            var father = node.Father;
            node.Right.Father = father;

            if (ReferenceEquals(Root, node))
            {
                Root = node.Right;
            }

            if (father is not null)
            {
                if (IsLeftChild(node))
                {
                    father.Left = node.Right;
                }
                else
                {
                    father.Right = node.Right;
                }
            }
        }
        // Two Children
        else
        {
            // predecessor key (right-most node in left subtree)
            var successorNode = node.Left!;
            while (successorNode.Right is not null)
            {
                successorNode = successorNode.Right;
            }

            node.Key = successorNode.Key;
            Delete(successorNode);
        }

        return true;
    }

    public virtual int GetHeight()
    {
        return GetHeight(Root);
    }

    public virtual int GetHeight(BinaryTreeNode? node)
    {
        if (node is null)
        {
            return 0;
        }

        return 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
    }

    protected static bool IsLeftChild(BinaryTreeNode? node)
    {
        if (node is null)
        {
            return false;
        }

        return node.Father is not null && ReferenceEquals(node, node.Father.Left);
    }
}
