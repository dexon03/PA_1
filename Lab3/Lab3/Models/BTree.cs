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

    public void Delete(Node current, int id)
    {
        int indexForDelete = current.NodeValues.TakeWhile(node => id.CompareTo(node.NodeValueId) > 0).Count();
        if (indexForDelete < current.NodeValues.Count &&
            current.NodeValues[indexForDelete].NodeValueId.CompareTo(id) == 0)
        {
            DeleteValueFromNodeValues(current, id, indexForDelete);
            return;
        }

        if (!current.IsLeaf)
        {
            DeleteKeyFromSubtree(current,id,indexForDelete);    
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
            NodeValue predecessor = this.DeletePredecessor(predecessorChild);
            node.NodeValues[indexForDelete] = predecessor;
        }
        else
        {
            Node successorChild = node.Children[indexForDelete + 1];
            if (successorChild.NodeValues.Count >= this.Degree)
            {
                NodeValue successor = DeleteSuccessor(successorChild);
                node.NodeValues[indexForDelete] = successor;
            }
            else
            {
                predecessorChild.NodeValues.Add(node.NodeValues[indexForDelete]);
                predecessorChild.NodeValues.AddRange(successorChild.NodeValues);
                predecessorChild.Children.AddRange(successorChild.Children);
                
                node.NodeValues.RemoveAt(indexForDelete);
                node.Children.RemoveAt(indexForDelete+1);
                
                Delete(predecessorChild,id);
            }
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
    
    private NodeValue DeleteSuccessor(Node node)
    {
        if (node.IsLeaf)
        {
            NodeValue result = node.NodeValues[0];
            node.NodeValues.RemoveAt(0);
            return result;
        }

        return DeleteSuccessor(node.Children.First());
    }

    private void DeleteKeyFromSubtree(Node parentNode, int keyToDelete, int subtreeIndexInNode)
    {
        Node childNode = parentNode.Children[subtreeIndexInNode];

        if (childNode.HasReachedMinCountOfKeys)
        {
            int leftIndex = subtreeIndexInNode - 1;
            Node leftSibling = subtreeIndexInNode > 0 ? parentNode.Children[leftIndex] : null;

            int rightIndex = subtreeIndexInNode + 1;
            Node rightSibling = subtreeIndexInNode < parentNode.Children.Count - 1
                ? parentNode.Children[rightIndex]
                : null;

            if (leftSibling != null && leftSibling.NodeValues.Count > this.Degree - 1)
            {
                childNode.NodeValues.Insert(0, parentNode.NodeValues[subtreeIndexInNode]);
                parentNode.NodeValues[subtreeIndexInNode] = leftSibling.NodeValues.Last();
                leftSibling.NodeValues.RemoveAt(leftSibling.NodeValues.Count - 1);

                if (!leftSibling.IsLeaf)
                {
                    childNode.Children.Insert(0, leftSibling.Children.Last());
                    leftSibling.Children.RemoveAt(leftSibling.Children.Count - 1);
                }
            }
            else if (rightSibling != null && rightSibling.NodeValues.Count > this.Degree - 1)
            {
                childNode.NodeValues.Add(parentNode.NodeValues[subtreeIndexInNode]);
                parentNode.NodeValues[subtreeIndexInNode] = rightSibling.NodeValues.First();
                rightSibling.NodeValues.RemoveAt(0);

                if (!rightSibling.IsLeaf)
                {
                    childNode.Children.Add(rightSibling.Children.First());
                    rightSibling.Children.RemoveAt(0);
                }
            }
            else
            {
                if (leftSibling != null)
                {
                    childNode.NodeValues.Insert(0, parentNode.NodeValues[subtreeIndexInNode]);
                    var oldNodeValues = childNode.NodeValues;
                    childNode.NodeValues = leftSibling.NodeValues;
                    childNode.NodeValues.AddRange(oldNodeValues);
                    if (!leftSibling.IsLeaf)
                    {
                        var oldChildren = childNode.Children;
                        childNode.Children = leftSibling.Children;
                        childNode.Children.AddRange(oldChildren);
                    }

                    parentNode.Children.RemoveAt(leftIndex);
                    parentNode.NodeValues.RemoveAt(subtreeIndexInNode);
                }
                else
                {
                    childNode.NodeValues.Add(parentNode.NodeValues[subtreeIndexInNode]);
                    childNode.NodeValues.AddRange(rightSibling.NodeValues);
                    if (!rightSibling.IsLeaf)
                    {
                        childNode.Children.AddRange(rightSibling.Children);
                    }

                    parentNode.Children.RemoveAt(rightIndex);
                    parentNode.NodeValues.RemoveAt(subtreeIndexInNode);
                }
            }
        }
        this.Delete(childNode, keyToDelete);
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