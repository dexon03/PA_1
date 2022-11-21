using Lab3.Data;

namespace Lab3.Models;

public class BTree
{
    public int Degree { get; set; } = 50;
    public Node Root { get; set; } = new Node(50);

    public BTree(IServiceScopeFactory _serviceScopeFactory)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            ApplicationDbContext dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            if (dbContext.NodeValues.Any())
            {
                foreach (var node in dbContext.NodeValues)
                {
                    BTreeInsert(node); 
                }
            }
        }
    }

    public NodeValue? BTreeSearch(Node current,int key)
    {
        var node = SearchNode(current, key);
        if (node is null) return null;
        return BinarySearch(node, key);
    }

    
    private Node? SearchNode(Node node, int key)
    {
        if (node.Find(key) != -1) return node;
        if (node.IsLeaf) return null;
        var nextNode = node.FindChildForKey(key);
        return SearchNode(nextNode, key);

    }
    
    private NodeValue? BinarySearch(Node node, int key)
    {
        int high = node.NodeValues.Count;
        int low = 0;
        while (low <= high)
        {
            int mid = (low + high) / 2;
            if (node.NodeValues[mid].NodeValueId == key) return node.NodeValues[mid];
            if (node.NodeValues[mid].NodeValueId > key) low = mid + 1;
            else
            {
                high = mid - 1;
            }
        }

        return null;
    }

    public void BTreeInsert(NodeValue node)
    {
        if (this.Root.HasReachedMaxCountOfKeys)
        {
            Node oldRoot = this.Root;
            this.Root = new Node(this.Degree);
            this.Root.Children.Add(oldRoot);
            this.SplitChild(this.Root,0,oldRoot);
            this.InsertNotFull(this.Root,node.NodeValueId,node.Value);
        }
        else
        {
            this.InsertNotFull(this.Root,node.NodeValueId,node.Value);
        }
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