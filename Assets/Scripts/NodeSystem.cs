using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeSystem : BaseSystem<NodeEntity>
{
    private int _lastId;
    private Dictionary<int, NodeEntity> _nodes = new();

    protected override void AddActor(NodeEntity actor)
    {
        _nodes.Add(_lastId, actor);
        actor.Id = _lastId;
        _lastId++;
        
        var nc = TeamManager.TeamControllers.Find(nc => nc.TeamId == actor.TeamId);
        if (nc == null)
        {
            if (LevelManager.IsNetwork)
            {
                nc = actor.TeamId switch
                {
                    0 => new NeutralController(0),
                    _ => new PlayerController(actor.TeamId)
                };
            }
            else
            {
                nc = actor.TeamId switch
                {
                    0 => new NeutralController(0),
                    1 => new PlayerController(1),
                    _ => new AiController(actor.TeamId)
                };
            }
            
            nc.Init();

            if (nc is IUpdatable u)
                WorldManager.CurrentWorld.GetSystem<UpdateSystem>().AddUpdatable(u);
            
            TeamManager.TeamControllers.Add(nc);
        }

        actor.UnitCount += nc.AdditionalInjection;
        actor.UnitReproductionTimeScale = nc.Reproduction;
        
        nc.Nodes.Add(actor);

        if (nc is PlayerController p && LevelManager.TeamId == p.TeamId)
            actor.PlayHighlighting();
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
        
        _nodes.Remove(actor.Id);
    }

    public void SendUnits(int id, List<UnitData> units, int targetId)
    {
        _nodes[id].SendUnits(units, targetId);
    }
    
    public void GetHit(int nodeId, UnitData unit)
    {
        var node = _nodes[nodeId];
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
        
        if (owner.Nodes.Count == 0)
        {
            if (owner is PlayerController)
            {
                if (LevelManager.IsNetwork)
                {
                    if (LevelManager.TeamId == owner.TeamId)
                        LevelManager.CompleteNetworkLevel(false);
                }
                else
                    LevelManager.CompleteLevel(false);
            }
        }

        if (TeamManager.TeamControllers.FindAll(nc =>  nc.TeamId != newOwner.TeamId && nc.TeamId != 0)
            .All(nc => nc.Nodes.Count == 0))
        {
            if (newOwner is PlayerController)
            {
                if (!LevelManager.IsNetwork)
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