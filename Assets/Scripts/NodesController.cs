using System.Collections.Generic;

public class NodesController
{
    public readonly int TeamId;
    public readonly List<NodeEntity> Nodes = new();

    protected NodesController(int teamId)
    {
        TeamId = teamId;
    }

    public virtual bool ActivateNode(NodeEntity node)
    {
        return false;
    }

    public virtual void SendUnits(NodeEntity targetNode)
    {
        
    }

    public virtual void SearchTarget()
    {
        
    }

    public virtual void StopSearching()
    {
        
    }

    public virtual void DeactivateNode(NodeEntity node)
    {
        
    }

    public virtual void DeactivateNodes()
    {
        
    }
}