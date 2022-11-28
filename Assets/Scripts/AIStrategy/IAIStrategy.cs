using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AIStrategy
{
    public abstract NodeEntity SearchTarget(AiController ai);
    
    public List<NodeEntity> FindInRadius(NodeEntity main, float radius)
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