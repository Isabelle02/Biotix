using System.Collections.Generic;

public class PlayerController : NodesController
{
    public readonly List<NodeEntity> SelectedNodes = new();

    public PlayerController(int teamId) : base(teamId)
    {
    }
    
    public override bool ActivateNode(NodeEntity node)
    {
        if (!Nodes.Contains(node))
            return false;
        
        if (SelectedNodes.Contains(node))
            return false;

        node.SetHighlighted(true);
        SelectedNodes.Add(node);
        return true;
    }

    public override void SendUnits(NodeEntity targetNode)
    {
        if (SelectedNodes.Contains(targetNode))
        {
            targetNode.SetLineActive(false);
            targetNode.SetHighlighted(false);
            SelectedNodes.Remove(targetNode);
        }

        foreach (var n in SelectedNodes)
        {
            n.SendUnits(targetNode);
            n.SetLineActive(false);
            n.SetHighlighted(false);
        }
        
        SelectedNodes.Clear();
    }

    public override void SearchTarget()
    {
        var node = MouseManager.GetObject<NodeView>();
        if (node != null)
            ActivateNode(node.NodeEntity);
        
        foreach (var n in SelectedNodes)
        {
            n.SetLineActive(true);
            n.SetLineEndPosition(MouseManager.GetMousePosition(0));
        }
    }

    public override void StopSearching()
    {
        foreach (var n in SelectedNodes)
        {
            n.SetLineActive(false);
        }
    }

    public override void DeactivateNode(NodeEntity node)
    {
        if (SelectedNodes.Contains(node))
        {
            node.SetLineActive(false);
            node.SetHighlighted(false);
            SelectedNodes.Remove(node);
        }
    }

    public override void DeactivateNodes()
    {
        foreach (var n in SelectedNodes)
        {
            n.SetLineActive(false);
            n.SetHighlighted(false);
        }
        
        SelectedNodes.Clear();
    }
}