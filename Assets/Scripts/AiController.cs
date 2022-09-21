using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiController : NodesController, IUpdatable
{
    private float _stepTimeScale = 4f;
    private float _stepPassTime;

    private NodeEntity _targetNode;
    
    public readonly List<NodeEntity> SelectedNodes = new();

    public AiController(int teamId) : base(teamId)
    {
    }
    
    public void Update(float delta)
    {
        _stepPassTime += delta;
        if (_stepPassTime >= _stepTimeScale)
        {
            if (Nodes.Count != 0)
            {
                SearchTarget();
                SendUnits(_targetNode);
                _stepPassTime = 0;
            }
        }
    }

    public override bool ActivateNode(NodeEntity node)
    {
        if (!Nodes.Contains(node))
            return false;
        
        if (SelectedNodes.Contains(node))
            return false;

        SelectedNodes.Add(node);
        return true;
    }

    public override void SendUnits(NodeEntity targetNode)
    {
        if (SelectedNodes.Contains(targetNode))
        {
            SelectedNodes.Remove(targetNode);
        }

        foreach (var n in SelectedNodes)
        {
            n.SendUnits(targetNode);
        }
        
        SelectedNodes.Clear();
    }

    public override void SearchTarget()
    {
        var count = Random.Range(1, Nodes.Count + 1);
        for (var i = 0; i < count; i++)
        {
            var node = Nodes[Random.Range(0, Nodes.Count)];
            if (!ActivateNode(node)) 
                i--;
        }
        
        var main = SelectedNodes[Random.Range(0, count)];
        var nearest = FindNearest(main, 100);
        _targetNode = nearest[Random.Range(0, nearest.Count)];
    }

    public override void DeactivateNode(NodeEntity node)
    {
        if (SelectedNodes.Contains(node))
        {
            SelectedNodes.Remove(node);
        }
    }

    public override void DeactivateNodes()
    {
        SelectedNodes.Clear();
    }

    private List<NodeEntity> FindNearest(NodeEntity main, float radius)
    {
        var cs = Physics2D.OverlapCircleAll(main.CurrentPosition, radius).ToList();
        var nearest = new List<NodeEntity>();
        foreach (var c in cs)
        {
            if (c.TryGetComponent<NodeView>(out var n))
                nearest.Add(n.NodeEntity);
        }

        return nearest;
    }
}