using Lab3.Data;

namespace Lab3.Models;

public class BTree
{
    public int Degree { get; set; }
    public Node Root { get; set; } = new Node(50);

    public BTree(ApplicationDbContext dbContext,int degree)
    {
        Degree = degree;
        if (dbContext.NodeValues.Any())
        {
            foreach (var node in dbContext.NodeValues)
            {
               BTreeInsert(node.NodeValueId,node.Value); 
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
            this.SplitChild(this.Root,0,oldRoot);
            this.InsertNotFull(this.Root,id,value);
        }
        else
        {
            this.InsertNotFull(this.Root,id,value);
        }
    }

    public void Delete(Node current, int id)
    {
        int indexForDelete = current.NodeValues.TakeWhile(node => id.CompareTo(node.NodeValueId) > 0).Count();
        if (indexForDelete < current.NodeValues.Count &&
            current.NodeValues[indexForDelete].NodeValueId.CompareTo(id) == 0)
        {
            DeleteValueFromNodeValues(current, id, indexForDelete);
        }
    }

    private void DeleteValueFromNodeValues(Node node, int id, int indexForDelete)
    {
        if (node.IsLeaf)
        {
            node.NodeValues.RemoveAt(indexForDelete);
            return;
        }

        Node predecessorChild = node.Children[indexForDelete];
        if (predecessorChild.NodeValues.Count >= this.Degree)
        {
            
        }
    }

    private NodeValue DeletePredecessor(Node node)
    {
        if (node.IsLeaf)
        {
            NodeValue result = node.NodeValues[^1];
            node.NodeValues.RemoveAt(node.NodeValues.Count-1);
            return result;
        }

        return DeletePredecessor(node.Children.Last());
    }
    private void SplitChild(Node parrent,int nodeIdToSplit,Node nodeForSplit)
    {
        Node newNode = new Node(this.Degree);
        parrent.NodeValues.Insert(nodeIdToSplit,nodeForSplit.NodeValues[this.Degree - 1]);
        parrent.Children.Insert(nodeIdToSplit + 1,newNode);
        
        newNode.NodeValues.AddRange(nodeForSplit.NodeValues.GetRange(this.Degree,this.Degree -1));
        nodeForSplit.NodeValues.RemoveRange(this.Degree-1,this.Degree);
        if (!nodeForSplit.IsLeaf)
        {
            newNode.Children.AddRange(nodeForSplit.Children.GetRange(this.Degree,this.Degree));
            nodeForSplit.Children.RemoveRange(this.Degree,this.Degree);
        }
    }

    private void InsertNotFull(Node node, int id, string value)
    {
        int indexForInsert = node.NodeValues.TakeWhile(node => id.CompareTo(node.NodeValueId) >= 0).Count();
        if (node.IsLeaf)
        {
            node.NodeValues.Insert(indexForInsert,new NodeValue(){NodeValueId = id,Value = value});
            return;
        }

        Node child = node.Children[indexForInsert];
        if (child.HasReachedMaxCountOfKeys)
        {
            SplitChild(node,indexForInsert,child);
            if (id.CompareTo(node.NodeValues[indexForInsert].NodeValueId) > 0) indexForInsert++;
        }
        InsertNotFull(node.Children[indexForInsert],id,value);
    }

    public List<NodeValue> ToList()
    {
        var nodes = new List<NodeValue>();
        ToList(this.Root, nodes);
        // nodes.Sort()
        return nodes;
    }

    private void ToList(Node node, List<NodeValue> nodes)
    {
        int i = 0;
        for (i = 0; i < node.NodeValues.Count; i++)
        {
            if (!node.IsLeaf)
            {
                ToList(node.Children[i], nodes);
            }
            nodes.Add(node.NodeValues[i]);
        }

        if (!node.IsLeaf)
        {
            ToList(node.Children[i],nodes);
        }
    }
}