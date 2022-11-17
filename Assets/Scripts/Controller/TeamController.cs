using System.Collections.Generic;

public class TeamController
{
    private readonly LaboratoryData _laboratoryData = new();
    
    public readonly int TeamId;
    public readonly List<NodeEntity> Nodes = new();

    public float Attack { get; protected set; }
    
    public float Defence { get; protected set; }
    
    public float Speed { get; protected set; }

    public float Reproduction { get; protected set; }

    public int AdditionalInjection { get; protected set; }

    protected TeamController(int teamId)
    {
        TeamId = teamId;
        Attack = _laboratoryData.BaseAttack;
        Defence = _laboratoryData.BaseDefence;
        Speed = _laboratoryData.BaseSpeed;
        Reproduction = _laboratoryData.BaseReproduction;
        AdditionalInjection = _laboratoryData.BaseAdditionalInjection;
    }

    public virtual void Init()
    {
        
    }

    public virtual void SetAttack(float rate)
    {
        
    }

    public virtual void SetDefence(float rate)
    {
        
    }
    
    public virtual void SetSpeed(float rate)
    {
        
    }
    
    public virtual void SetReproduction(float rate)
    {
        
    }
    
    public virtual void SetAdditionalInjection(float rate)
    {
        
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

    public virtual void Dispose()
    {
        
    }
}