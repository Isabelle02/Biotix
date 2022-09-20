using System.Collections.Generic;

public class PlayerSystem : BaseSystem<PlayerEntity>
{
    private readonly List<PlayerEntity> _players = new();

    protected override void AddActor(PlayerEntity actor)
    {
        _players.Add(actor);
    }

    protected override void RemoveActor(PlayerEntity actor)
    {
        _players.Remove(actor);
    }

    public void HandleSelectedNodesLines(bool isLineActive)
    {
        foreach (var p in _players)
        {
            foreach (var n in p.SelectedNodes)
            {
                n.SetLineActive(isLineActive);
                n.SetLineEndPosition(MouseManager.GetMousePosition(0));
            }
        }
    }

    public bool ActivateNode(NodeEntity node)
    {
        var p = _players.Find(p => p.Nodes.Contains(node));
        if (p == null)
            return false;

        if (p.SelectedNodes.Contains(node))
            return false;

        node.SetHighlighted(true);
        p.SelectedNodes.Add(node);
        return true;
    }

    public void SendUnits(NodeEntity node)
    {
        foreach (var p in _players)
        {
            if (p.SelectedNodes.Contains(node))
            {
                node.SetLineActive(false);
                node.SetHighlighted(false);
                p.SelectedNodes.Remove(node);
            }

            foreach (var n in p.SelectedNodes)
            {
                n.SendUnits();
                n.SetLineActive(false);
                n.SetHighlighted(false);
            }
            
            p.SelectedNodes.Clear();
        }
    }

    public void ResetSelectedNodes()
    {
        foreach (var p in _players)
        {
            foreach (var n in p.SelectedNodes)
            {
                n.SetLineActive(false);
                n.SetHighlighted(false);
            }
        }
        
        foreach (var p in _players) 
            p.SelectedNodes.Clear();
    }
}