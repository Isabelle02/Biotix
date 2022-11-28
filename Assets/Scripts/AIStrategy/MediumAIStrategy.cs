using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MediumAIStrategy : AIStrategy
{
    public override NodeEntity SearchTarget(AiController ai)
    {
        var selectedNodes = new List<NodeEntity>();
        var unitsSum = ai.Nodes.Select(n => n.UnitCount).Sum();
        var minArmyCount = 0.4f * unitsSum;
        var armyCount = 0;
        var tries = 3;
        while (armyCount < minArmyCount && tries > 0)
        {
            var node = ai.Nodes[Random.Range(0, ai.Nodes.Count)];
            if (!ai.ActivateNode(node))
            {
                tries--;
                continue;
            }

            selectedNodes.Add(node);
            armyCount += node.UnitCount / 2;
        }
        
        var main = selectedNodes[Random.Range(0, selectedNodes.Count)];
        var nearest = FindInRadius(main, Random.Range(1, 15));
        return nearest.Find(node => node.UnitCount == nearest.Select(n => n.UnitCount).Min());
    }
}