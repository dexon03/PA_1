using Lab3.Data;

namespace Lab3.Models;

public class BTree
{
    public int Degree { get; set; }
    public Node Root { get; set; }

    public BTree(ApplicationDbContext dbContext,int degree)
    {
        Degree = degree;
        if (dbContext.NodeValues != null)
        {
            foreach (var node in dbContext.NodeValues)
            {
               this.BTreeInsert(node.NodeValueId,node.Value); 
            }
        }
    }

    public NodeValue? BTreeSearch(Node current,int id)
    {
        int i = 0;
        while (i <= current.NodeValues.Count && id > current.NodeValues[i].NodeValueId)
        {
            i++;
        }

        if (i <= current.NodeValues.Count && id == current.NodeValues[i].NodeValueId)
        {
            return current.NodeValues[i];
        }

        return current.IsLeaf ? null : BTreeSearch(current.Children[i], id);
    }

    public void BTreeInsert(int id, string value)
    {
        if (this.Root.HasReachedMaxCountOfKeys)
        {
            Node oldRoot = this.Root;
            this.Root = new Node(this.Degree);
            this.Root.Children.Add(oldRoot);
            this.Root.IsLeaf = false;
            this.SplitChild(this.Root,0,oldRoot);
            this.InsertNotFull(this.Root,id,value);
        }
        else
        {
            this.InsertNotFull(this.Root,id,value);
        }
    } 
    
    // public void Delete
    
    private void SplitChild(Node parrent,int nodeIdToSplit,Node nodeForSplit)
    {
        Node newNode = new Node(this.Degree);
        parrent.NodeValues.Insert(nodeIdToSplit,nodeForSplit.NodeValues[this.Degree - 1]);
        parrent.Children.Insert(nodeIdToSplit + 1,newNode);
        
        newNode.NodeValues.AddRange(nodeForSplit.NodeValues.GetRange(this.Degree,this.Degree -1));
        nodeForSplit.NodeValues.RemoveRange(this.Degree-1,this.Degree);
        if (nodeForSplit.IsLeaf)
        {
            newNode.Children.AddRange(nodeForSplit.Children.GetRange(this.Degree,this.Degree));
            nodeForSplit.Children.RemoveRange(this.Degree,this.Degree);
        }
    }

    private void InsertNotFull(Node node, int id, string value)
    {
        int indexForInsert = 0;
        while (indexForInsert <= node.NodeValues.Count && id > node.NodeValues[indexForInsert].NodeValueId)
        {
            indexForInsert++;
        }

        if (node.IsLeaf)
        {
            node.NodeValues.Insert(indexForInsert, new NodeValue{NodeValueId = id,Value = value});
        }

        Node child = node.Children[indexForInsert];
        if (child.HasReachedMaxCountOfKeys)
        {
            this.SplitChild(node,indexForInsert,child);
            if (id > node.NodeValues[indexForInsert].NodeValueId)
            {
                indexForInsert++;
            }
        }
        
        this.InsertNotFull(node.Children[indexForInsert],id,value);
    }
}