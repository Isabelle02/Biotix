using System.Collections.Generic;

public class NodeSystem : BaseSystem<NodeEntity>
{
    private readonly List<NodesController> _nodesControllers = new();
    
    public readonly List<NodeEntity> Nodes = new();

    protected override void AddActor(NodeEntity actor)
    {
        var nc = _nodesControllers.Find(nc => nc.TeamId == actor.TeamId);
        if (nc == null)
        {
            nc = actor.TeamId switch
            {
                0 => new NeutralController(0),
                1 => new PlayerController(1),
                _ => new AiController(actor.TeamId)
            };

            if (nc is IUpdatable u)
                WorldManager.CurrentWorld.GetSystem<UpdateSystem>().AddUpdatable(u);
            
            _nodesControllers.Add(nc);
        }

        nc.Nodes.Add(actor);
        Nodes.Add(actor);
    }

    protected override void RemoveActor(NodeEntity actor)
    {
        foreach (var nc in _nodesControllers)
            if (nc.Nodes.Contains(actor))
            {
                nc.DeactivateNode(actor);
                nc.Nodes.Remove(actor);
            }
    }
    
    public void UpdateNodes(NodeEntity node)
    {
        var owner = _nodesControllers.Find(nc => nc.Nodes.Contains(node));
        owner.DeactivateNode(node);
        owner.Nodes.Remove(node);

        var newOwner = _nodesControllers.Find(nc => nc.TeamId == node.TeamId);
        newOwner.Nodes.Add(node);

        var aliveControllers = _nodesControllers.FindAll(nc => nc.Nodes.Count > 0);
        if (aliveControllers.Count == 1 && aliveControllers[0].TeamId == 1)
            PopupManager.Open<MatchCompletionPopup>(new MatchCompletionPopup.Param(true));
        else
        {
            var player = _nodesControllers.Find(nc => nc.TeamId == 1);
            if (player.Nodes.Count == 0)
                PopupManager.Open<MatchCompletionPopup>(new MatchCompletionPopup.Param(false));
        }
    }

    public void SearchTarget(int teamId)
    {
        foreach (var nc in _nodesControllers)
        {
            if (nc.TeamId != teamId)
                continue;
            
            nc.SearchTarget();
        }
    }

    public void StopSearching(int teamId)
    {
        foreach (var nc in _nodesControllers)
        {
            if (nc.TeamId != teamId)
                continue;
            
            nc.StopSearching();
        }
    }
    
    public bool ActivateNode(int teamId, NodeEntity node)
    {
        foreach (var nc in _nodesControllers)
        {
            if (nc.TeamId != teamId)
                continue;

            return nc.ActivateNode(node);
        }

        return false;
    }

    public void SendUnits(int teamId, NodeEntity target)
    {
        foreach (var nc in _nodesControllers)
        {
            if (nc.TeamId != teamId)
                continue;
            
            nc.SendUnits(target);
        }
    }

    public void DeactivateNodes(int teamId)
    {
        foreach (var nc in _nodesControllers)
        {
            if (nc.TeamId != teamId)
                continue;
            
            nc.DeactivateNodes();
        }
    }
}