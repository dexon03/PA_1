namespace Lab3.Models;

public class Node
{
    // public int numOfKeys;
    public int degree;
    public List<NodeValue> NodeValues { get; set; }
    public bool IsLeaf { get; set; }
    public List<Node> Children { get; set; }

    public Node(int degree)
    {
        this.degree = degree;
        NodeValues = new List<NodeValue>(this.degree);
        Children = new List<Node>(this.degree);
    }

    public bool HasReachedMaxCountOfKeys
    {
        get { return this.NodeValues.Count == ((this.degree * 2) - 1);}  
    }
    public bool HasReachedMinCountOfKeys
    {
        get { return this.NodeValues.Count == (this.degree  - 1);}  
    }
}