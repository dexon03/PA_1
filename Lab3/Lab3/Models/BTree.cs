using Lab3.Data;

namespace Lab3.Models;

public class BTree
{
    private int Degree { get; set; } = 50;
    public Node Root { get; set; } = new (50);

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

    public NodeValue? BTreeSearch(int key,ref int countOfComparsion)
    {
        var node = SearchNode(Root, key, ref countOfComparsion);
        if (node is null) return null;
        return BinarySearch(node.NodeValues, key, ref countOfComparsion);
    }

    
    private Node? SearchNode(Node node, int key, ref int countOfComparsion )
    {
        if (node.Find(key) != -1) return node;
        if (node.IsLeaf) return null;
        var nextNode = node.FindChildForKey(key,ref countOfComparsion);
        return SearchNode(nextNode, key, ref countOfComparsion);

    }
    
    private NodeValue? BinarySearch(List<NodeValue> nodeValues, int key, ref int countOfComparsion)
    {
        int position = (nodeValues.Count-1) / 2;
        int step = position;
        while (step > 0)
        {
            step = (int)Math.Ceiling((decimal)step / 2);
            countOfComparsion++;
            if (nodeValues[position].NodeValueId == key)
            {
                return nodeValues[position];
            }
            countOfComparsion++;
            if (nodeValues[position].NodeValueId > key)
            {
                position -= step;
            }else if(nodeValues[position].NodeValueId < key){
                position += step;
            }
            countOfComparsion++;
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
            this.Root.Children[this.Root.Children.IndexOf(oldRoot)].Parent = this.Root;
            this.SplitChild(this.Root,0,oldRoot);
            this.InsertNotFull(this.Root,node.NodeValueId,node.Value);
        }
        else
        {
            this.InsertNotFull(this.Root,node.NodeValueId,node.Value);
        }
    }

    private void SplitChild(Node parent,int nodeIdToSplit,Node nodeForSplit)
    {
        Node newNode = new Node(this.Degree);
        parent.NodeValues.Insert(nodeIdToSplit,nodeForSplit.NodeValues[this.Degree - 1]);
        parent.Children.Insert(nodeIdToSplit + 1,newNode);
        newNode.Parent = parent;
        newNode.NodeValues.AddRange(nodeForSplit.NodeValues.GetRange(this.Degree,this.Degree -1));
        nodeForSplit.NodeValues.RemoveRange(this.Degree-1,this.Degree);
        if (!nodeForSplit.IsLeaf)
        {
            newNode.Children.AddRange(nodeForSplit.Children.GetRange(this.Degree,this.Degree));
            foreach (var child in newNode.Children)
            {
                child.Parent = newNode;
            }
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
        child.Parent = node;
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

    public void DeleteNode(int key)
    {
        int countOfComparsion = 0;
        var node = SearchNode(Root, key,ref countOfComparsion);
        if (node.IsLeaf)
        {
            RemoveNodeFromLeaf(node,key);
        }
        else
        {
            RemoveNodeFromNonLeaf(node,key);
        }
        
    }
    
    private void RemoveNodeFromLeaf(Node node, int key)
    {
        int countOfComparsion = 0;
        var nodeValueForRemove = BinarySearch(node.NodeValues, key,ref countOfComparsion);
        node.NodeValues.Remove(nodeValueForRemove);
        RestorePropertyDelete(node);
    }

    private void RemoveNodeFromNonLeaf(Node node,int key)
    {
        int countOfCompersion = 0;
        var leftChild = node.FindChildForKey(key,ref countOfCompersion);
        if (leftChild.NodeValues.Count > Degree - 1)
        {
            var predecessor = GetPredecessor(leftChild);
            var predecessorValue = predecessor.NodeValues[^1];
            node.ReplaceValueByKey(key,predecessorValue);
            RemoveNodeFromLeaf(predecessor,predecessorValue.NodeValueId);
        }
        else
        {
            var rightChild = leftChild.rightSibling;
            var successor = GetSuccessor(rightChild);
            var successorKey = successor.NodeValues[0];
            node.ReplaceValueByKey(key,successorKey);
            RemoveNodeFromLeaf(successor,successorKey.NodeValueId);
        }
    }
    private void RestorePropertyDelete(Node node)
    {
        if (node.NodeValues.Count < Degree - 1)
        {
            if (node == Root)
            {
                if (node.NodeValues.Count == 0 && node.Children.Count > 0)
                {
                    Root = node.Children[0];
                    node.Children.Remove(Root);
                }
                else if (node.NodeValues.Count == 0)
                {
                    Root = null;
                }
            }
            else if (!BorrowLeft(node) && !BorrowRight(node))
            {
                Merge(node);
                RestorePropertyDelete(node.Parent);
            }            
        }
    }

    private Node GetPredecessor(Node node)
    {
        while (!node.IsLeaf)
        {
            node = node.Children[^1];
        }
        return node;
    }

    private Node GetSuccessor(Node node)
    {
        while (!node.IsLeaf)
        {
            node = node.Children[0];
        }
        return node;
    }

    private bool BorrowLeft(Node node)
    {
        var left = node.leftSibling;
        var parent = node.Parent;
        if (left != null && left.NodeValues.Count > Degree - 1)
        {
            var sibKey = left.ExtractLastKey();
            var parentKey = parent.FindKeyByChild(left);
            parent.ReplaceValueByKey(parentKey.NodeValueId,sibKey);
            node.NodeValues.Insert(0,parentKey);
            if (!left.IsLeaf)
            {
                var sibChild = left.ExtractLastChild();
                node.Children.Insert(0, sibChild); 
                node.Children[0].Parent = node;
            }

            return true;
        }
        return false;
    }
    private bool BorrowRight(Node node)
    {
        var right = node.rightSibling;
        var parent = node.Parent;
        if (right != null && right.NodeValues.Count > Degree - 1)
        {
            var sibKey = right.ExtractFirstKey();
            var parentKey = parent.FindKeyByChild(right);
            parent.ReplaceValueByKey(parentKey.NodeValueId,sibKey); 
            node.NodeValues.Add(parentKey);
            if (!right.IsLeaf)
            {
                var sibChild = right.ExtractFirstChild();
                node.Children.Add(sibChild);
                node.Children[^1].Parent = node;
            }

            return true;
        }
        return false;
    }
    private void Merge(Node node)
    {
        var left = node.leftSibling;
        var parent = node.Parent;
        if (left != null)
        {
            var parentKey = parent.FindKeyByChild(left);
            parent.NodeValues.Remove(parentKey);
            node.Children.InsertRange(0,left.Children);
            for (int i = 0; i < left.Children.Count; i++)
            {
                node.Children[i].Parent = node;
            }
            parent.Children.Remove(left);
            node.NodeValues.Insert(0,parentKey);
            node.NodeValues.InsertRange(0,left.NodeValues);
        }
        else
        {
            var right = node.rightSibling;
            var parentKey = parent.FindKeyByChild(node);
            parent.NodeValues.Remove(parentKey);
            node.Children.AddRange(right.Children);
            foreach (var child in node.Children)
            {
                child.Parent = node;
            }

            parent.Children.Remove(right);
            node.NodeValues.Add(parentKey);
            node.NodeValues.AddRange(right.NodeValues);
        }
    }

    public void Edit(NodeValue nodeValue)
    {
        int countOfComparsion = 0;
        var node = SearchNode(Root, nodeValue.NodeValueId, ref countOfComparsion);
        var nodeForEdit = BinarySearch(node.NodeValues, nodeValue.NodeValueId,ref countOfComparsion);
        if (nodeForEdit != null) nodeForEdit.Value = nodeValue.Value;
    }
}