using Binary.Tree.Domain.Structures;

namespace Binary.Tree.Domain;

public class AvlTree : BinaryTree
{
    public override void Insert(int key)
    {
        var node = new BinaryTreeNode(key);
        base.Insert(node);

        var father = node.Father;

        while (father is not null)
        {
            var balanceFactor = GetBalanceFactor(father);
            if (Math.Abs(balanceFactor) == 2)
            {
                Balance(father, balanceFactor);
            }

            father = father.Father;
        }
    }

    public override bool Delete(int key)
    {
        var node = Search(key);
        return Delete(node);
    }

    public override bool Delete(BinaryTreeNode? node)
    {
        var father = node?.Father;
        var deleted = base.Delete(node);

        if (!deleted)
        {
            return false;
        }

        while (father is not null)
        {
            var balanceFactor = GetBalanceFactor(father);

            if (Math.Abs(balanceFactor) == 1)
            {
                break;
            }
            if (Math.Abs(balanceFactor) == 2)
            {
                Balance(father, balanceFactor);
            }

            father = father.Father;
        }

        return true;
    }

    private int GetBalanceFactor(BinaryTreeNode node)
    {
        return GetHeight(node.Right) - GetHeight(node.Left);
    }

    private void Balance(BinaryTreeNode node, int balanceFactor)
    {
        if (balanceFactor == 2)
        {
            var rightBalanceFactor = GetBalanceFactor(node.Right!);
            if (rightBalanceFactor == 1 || rightBalanceFactor == 0)
            {
                LeftRotate(node);
            }
            else if (rightBalanceFactor == -1)
            {
                RightRotate(node.Right!);
                LeftRotate(node);
            }
        }

        if (balanceFactor == -2)
        {
            var leftBalanceFactor = GetBalanceFactor(node.Left!);
            if (leftBalanceFactor == 1)
            {
                LeftRotate(node.Left!);
                RightRotate(node);
            }
            else if (leftBalanceFactor == -1 || leftBalanceFactor == 0)
            {
                RightRotate(node);
            }
        }
    }

    private void LeftRotate(BinaryTreeNode node)
    {
        if (node is null)
        {
            return;
        }

        var pivot = node.Right;

        if (pivot is null)
        {
            return;
        }

        var father = node.Father!;
        var isLeftChild = father is not null && ReferenceEquals(father.Left, node);
        var makeTreeRoot = ReferenceEquals(node.Tree.Root, node);

        // Rotate
        node.Right = pivot.Left;
        pivot.Left = node;

        // Update fathers
        node.Father = pivot;
        pivot.Father = father;

        if (node.Right is not null)
        {
            node.Right.Father = node;
        }

        if (makeTreeRoot)
        {
            pivot.Tree.Root = pivot;
        }

        // Update the original parent's child node
        if (isLeftChild)
        {
            father.Left = pivot;
            return;
        }

        if (father is not null)
        {
            father.Right = pivot;
        }
    }

    private void RightRotate(BinaryTreeNode node)
    {
        if (node is null)
        {
            return;
        }

        var pivot = node.Left;

        if (pivot is null)
        {
            return;
        }

        var father = node.Father!;
        var isLeftChild = father is not null && ReferenceEquals(father.Left, node);
        var makeTreeRoot = ReferenceEquals(node.Tree.Root, node);

        // Rotate
        node.Left = pivot.Right;
        pivot.Right = node;

        // Update fathers
        node.Father = pivot;
        pivot.Father = father;

        if (node.Left is not null)
        {
            node.Left.Father = node;
        }

        if (makeTreeRoot)
        {
            pivot.Tree.Root = pivot;
        }

        // Update the original parent's child node
        if (isLeftChild)
        {
            father.Left = pivot;
            return;
        }

        if (father is not null)
        {
            father.Right = pivot;
        }
    }
}
