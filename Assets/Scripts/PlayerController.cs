using System.Collections.Generic;

public class PlayerController : TeamController
{
    private readonly List<NodeEntity> _selectedNodes = new();

    public PlayerController(int teamId) : base(teamId)
    {
    }

    public override void Init()
    {
        SetAttack(PlayerLaboratoryManager.AttackRate);
        SetDefence(PlayerLaboratoryManager.DefenceRate);
        SetSpeed(PlayerLaboratoryManager.SpeedRate);
        SetReproduction(PlayerLaboratoryManager.ReproductionRate);
        SetAdditionalInjection(PlayerLaboratoryManager.AdditionalInjectionRate);
    }

    public override void SetAttack(float rate)
    {
        Attack += rate;
    }

    public override void SetDefence(float rate)
    {
        Defence += rate;
    }
    
    public override void SetSpeed(float rate)
    {
        Speed += rate;
    }
    
    public override void SetReproduction(float rate)
    {
        Reproduction -= rate;
    }
    
    public override void SetAdditionalInjection(float rate)
    {
        AdditionalInjection += (int) (rate * 10);
    }
    
    public override bool ActivateNode(NodeEntity node)
    {
        if (!Nodes.Contains(node))
            return false;
        
        if (_selectedNodes.Contains(node))
            return false;

        node.SetHighlighted(true);
        _selectedNodes.Add(node);
        return true;
    }

    public override void SendUnits(NodeEntity targetNode)
    {
        if (_selectedNodes.Contains(targetNode))
        {
            targetNode.SetLineActive(false);
            targetNode.SetHighlighted(false);
            _selectedNodes.Remove(targetNode);
        }

        foreach (var n in _selectedNodes)
        {
            n.SendUnits(targetNode, Attack, Speed);
            n.SetLineActive(false);
            n.SetHighlighted(false);
        }
        
        _selectedNodes.Clear();
    }

    public override void SearchTarget()
    {
        var node = MouseManager.GetObject<NodeView>();
        if (node != null)
            ActivateNode(node.NodeEntity);
        
        foreach (var n in _selectedNodes)
        {
            n.SetLineActive(true);
            n.SetLineEndPosition(MouseManager.GetMousePosition(0));
        }
    }

    public override void StopSearching()
    {
        foreach (var n in _selectedNodes)
        {
            n.SetLineActive(false);
        }
    }

    public override void DeactivateNode(NodeEntity node)
    {
        if (_selectedNodes.Contains(node))
        {
            node.SetLineActive(false);
            node.SetHighlighted(false);
            _selectedNodes.Remove(node);
        }
    }

    public override void DeactivateNodes()
    {
        foreach (var n in _selectedNodes)
        {
            n.SetLineActive(false);
            n.SetHighlighted(false);
        }
        
        _selectedNodes.Clear();
    }
}