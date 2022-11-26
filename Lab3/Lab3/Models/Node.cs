namespace Lab3.Models;

public class Node
{
    public int degree;
    public List<NodeValue> NodeValues { get; set; }
    public List<Node> Children { get; set; }

    public Node(int degree)
    {
        this.degree = degree;
        NodeValues = new List<NodeValue>(this.degree);
        Children = new List<Node>(this.degree);
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
    public bool HasReachedMinCountOfKeys
    {
        get { return this.NodeValues.Count == (this.degree  - 1);}  
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

    public Node FindChildForKey(int key)
    {
        for (int i = 0; i < NodeValues.Count; i++)
        {
            if (NodeValues[i].NodeValueId > key)
            {
                return Children[i];
            }
        }


        return Children[^1];
    }
}