namespace Lab3.Models;

public class Node
{
    public int degree;
    public List<NodeValue> NodeValues { get; set; }
    public List<Node> Children { get; set; }
    public Node? Parent { get; set; }
    public Node(int degree)
    {
        this.degree = degree;
        NodeValues = new List<NodeValue>(this.degree);
        Children = new List<Node>(this.degree);
        foreach (var child in Children)
        {
            child.Parent = this;
        }
    }
    public bool IsLeaf
    {
        get
        {
            return !this.Children.Any();
        }
    }
    public bool HasReachedMaxCountOfKeys
    {
        get { return this.NodeValues.Count == ((this.degree * 2) - 1);}  
    }
    public int Find(int id)
    {
        for (int i = 0; i < this.NodeValues.Count; i++)
        {
            if (this.NodeValues[i].NodeValueId == id)
            {
                return i;
            }
        }
        return -1;
    }

    public Node FindChildForKey(int key,ref int countOfComparsion)
    {
        for (int i = 0; i < NodeValues.Count; i++)
        {
            countOfComparsion++;
            if (NodeValues[i].NodeValueId > key)
            {
                return Children[i];
            }
        }
        return Children[^1];
    }
    public NodeValue FindKeyByChild(Node node)
    {
        var index = Math.Min(NodeValues.Count - 1, Children.IndexOf(node));
        return NodeValues[index];
    }
    public NodeValue ExtractLastKey()
    {
        var node = NodeValues[^1];
        NodeValues.Remove(node);
        return node;
    }
    public NodeValue ExtractFirstKey()
    {
        var node = NodeValues[0];
        NodeValues.Remove(node);
        return node;
    }
    public Node ExtractLastChild()
    {
        var node = Children[^1];
        Children.Remove(node);
        return node;
    }
    public Node ExtractFirstChild()
    {
        var node = Children[0];
        Children.Remove(node);
        return node;
    }
    private int IndexOfValueInChildren
    {
        get
        {
            if (Parent == null) return -1;
            else
            {
                return Parent.Children.IndexOf(this);
            }
        }
    }
    public Node? leftSibling
    {
        get
        {
            if (this.Parent == null || IndexOfValueInChildren == 0) return null;
            return Parent.Children[IndexOfValueInChildren - 1];
        }
    }
    public Node? rightSibling
    {
        get
        {
            if (this.Parent == null || IndexOfValueInChildren == Parent.Children.Count - 1) return null;
            return Parent.Children[IndexOfValueInChildren + 1];
        }
    }

    public void ReplaceValueByKey(int key, NodeValue value)
    {
        int index = NodeValues.FindIndex(u => u.NodeValueId == key);
        NodeValues[index] = value;
    }
}