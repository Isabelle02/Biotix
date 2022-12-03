using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HardAIStrategy : AIStrategy
{
    public override NodeEntity SearchTarget(AiController ai)
    {       
        var selectedNodes = new List<NodeEntity>();
        var unitsSum = ai.Nodes.Select(n => n.UnitCount).Sum();
        var minArmyCount = 0.3f * unitsSum;
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
        
        if (selectedNodes.Count == 0)
            return null;
        
        var main = selectedNodes[Random.Range(0, selectedNodes.Count)];
        var nearest = FindInRadius(main, Random.Range(1, 20));

        return nearest.Count == 0 ? null : nearest.Find(node => node.UnitCount <= 0.5f * armyCount);
    }
}