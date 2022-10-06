using System;
using System.Linq;
using UnityEngine;

public class NodeSystem : BaseSystem<NodeEntity>
{
    protected override void AddActor(NodeEntity actor)
    {
        var nc = TeamManager.TeamControllers.Find(nc => nc.TeamId == actor.TeamId);
        if (nc == null)
        {
            nc = actor.TeamId switch
            {
                0 => new NeutralController(0),
                1 => new PlayerController(1),
                _ => new AiController(actor.TeamId)
            };
            
            nc.Init();

            if (nc is IUpdatable u)
                WorldManager.CurrentWorld.GetSystem<UpdateSystem>().AddUpdatable(u);
            
            TeamManager.TeamControllers.Add(nc);
        }

        actor.UnitCount += nc.AdditionalInjection;
        actor.UnitReproductionTimeScale = nc.Reproduction;
        
        nc.Nodes.Add(actor);
    }

    protected override void RemoveActor(NodeEntity actor)
    {
        for (var i = 0; i < TeamManager.TeamControllers.Count; i++)
        {
            var nc = TeamManager.TeamControllers[i];
            if (nc.Nodes.Contains(actor))
            {
                nc.DeactivateNode(actor);
                nc.Nodes.Remove(actor);
            }

            if (nc.Nodes.Count == 0)
            {
                TeamManager.TeamControllers.RemoveAt(i);
                i--;
            }
        }
    }

    public void GetHit(NodeEntity node, UnitEntity unit)
    {
        if (node.TeamId == unit.TeamId)
        {
            node.UnitCount++;
        }
        else
        {
            var nc = TeamManager.TeamControllers.Find(nc => nc.Nodes.Contains(node));
            var armyForce = nc.Defence * node.UnitCount - unit.Attack;
            var unitCount = Mathf.RoundToInt(armyForce / nc.Defence);
            
            if (unitCount == 0)
                node.TeamId = 0;
            else if (unitCount < 0)
                node.TeamId = unit.TeamId;
            
            node.UnitCount = Math.Abs(unitCount);
            
            UpdateNodes(node);
        }
    }

    private void UpdateNodes(NodeEntity node)
    {
        var owner = TeamManager.TeamControllers.Find(nc => nc.Nodes.Contains(node));
        owner.DeactivateNode(node);
        owner.Nodes.Remove(node);

        var newOwner = TeamManager.TeamControllers.Find(nc => nc.TeamId == node.TeamId);
        newOwner.Nodes.Add(node);
        
        var player = TeamManager.TeamControllers.Find(nc => nc.TeamId == 1);
        if (player.Nodes.Count == 0)
        {
            LevelManager.CompleteLevel(false);
        }
        else
        {
            if (TeamManager.TeamControllers.All(nc => nc.TeamId is 0 or 1 && nc.Nodes.Count > 0 ||
                                                      nc.TeamId > 1 && nc.Nodes.Count == 0))
            {
                LevelManager.CompleteLevel(true);
            }
        }
    }

    public void SearchTarget(int teamId)
    {
        var nc = TeamManager.TeamControllers.Find(nc => nc.TeamId == teamId);
        if (nc == null)
            return;
        
        nc.SearchTarget();
    }

    public void StopSearching(int teamId)
    {
        var nc = TeamManager.TeamControllers.Find(nc => nc.TeamId == teamId);
        if (nc == null)
            return;
        
        nc.StopSearching();
    }
    
    public bool ActivateNode(int teamId, NodeEntity node)
    {
        var nc = TeamManager.TeamControllers.Find(nc => nc.TeamId == teamId);
        if (nc == null)
            return false;
        
        return nc.ActivateNode(node);
    }

    public void SendUnits(int teamId, NodeEntity target)
    {
        var nc = TeamManager.TeamControllers.Find(nc => nc.TeamId == teamId);
        if (nc == null)
            return;
        
        nc.SendUnits(target);
    }

    public void DeactivateNodes(int teamId)
    {
        var nc = TeamManager.TeamControllers.Find(nc => nc.TeamId == teamId);
        if (nc == null)
            return;
        
        nc.DeactivateNodes();
    }
}