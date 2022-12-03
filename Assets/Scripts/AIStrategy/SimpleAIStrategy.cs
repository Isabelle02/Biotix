using System.Collections.Generic;
using UnityEngine;

public class SimpleAIStrategy : AIStrategy
{
    public override NodeEntity SearchTarget(AiController ai)
    {
        var selectedNodes = new List<NodeEntity>();
        var count = Random.Range(1, ai.Nodes.Count + 1);
        var tries = 3;
        for (var i = 0; i < count && tries > 0; i++)
        {
            var node = ai.Nodes[Random.Range(0, ai.Nodes.Count)];
            if (!ai.ActivateNode(node))
            {
                i--;
                tries--;
            }
            else 
                selectedNodes.Add(node);
        }

        if (selectedNodes.Count == 0)
            return null;
        
        var main = selectedNodes[Random.Range(0, selectedNodes.Count)];
        var nearest = FindInRadius(main, Random.Range(1, 10));

        return nearest.Count == 0 ? null : nearest[Random.Range(0, nearest.Count)];
    }
}