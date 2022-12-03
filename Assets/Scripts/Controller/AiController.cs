using System.Collections.Generic;

public class AiController : TeamController, IUpdatable
{
    private float _stepTimeScale = 4f;
    private float _stepPassTime;

    private NodeEntity _targetNode;

    private readonly List<NodeEntity> _selectedNodes = new();

    private AIStrategy _strategy;

    public AiController(int teamId) : base(teamId)
    {
    }

    public override void Init()
    {
        var complexity = LevelManager.CurrentLevelIndex / LevelManager.LevelsCount * 2;
        _strategy = complexity switch
        {
            0 => new SimpleAIStrategy(),
            1 => new MediumAIStrategy(),
            _ => new HardAIStrategy()
        };

        _stepTimeScale -= LevelManager.CurrentLevelIndex / 100f;
        SetAttack(LevelManager.CurrentLevelIndex / 10f);
        SetDefence(LevelManager.CurrentLevelIndex / 10f);
        SetSpeed(LevelManager.CurrentLevelIndex / 100f);
        SetReproduction(LevelManager.CurrentLevelIndex / 10f);
    }

    public override void SetAttack(float rate)
    {
        Attack += rate;
    }

    public override void SetDefence(float rate)
    {
        Defence += rate;
    }
    
    public override void SetSpeed(float rate)
    {
        Speed += rate;
    }
    
    public override void SetReproduction(float rate)
    {
        Reproduction -= rate;
    }
    
    public void Update(float delta)
    {
        _stepPassTime += delta;
        if (_stepPassTime >= _stepTimeScale)
        {
            if (Nodes.Count != 0)
            {
                SearchTarget();
                if (_targetNode == null)
                {
                    _selectedNodes.Clear();
                    return;
                }

                SendUnits(_targetNode);
                _stepPassTime = 0;
            }
        }
    }

    public override bool ActivateNode(NodeEntity node)
    {
        if (!Nodes.Contains(node))
            return false;
        
        if (_selectedNodes.Contains(node))
            return false;

        _selectedNodes.Add(node);
        return true;
    }

    public override void SendUnits(NodeEntity targetNode)
    {
        if (_selectedNodes.Contains(targetNode))
        {
            _selectedNodes.Remove(targetNode);
        }

        foreach (var n in _selectedNodes)
        {
            n.SendUnits(targetNode, Attack, Speed);
        }
        
        _selectedNodes.Clear();
    }

    public override void SearchTarget()
    {
        _targetNode = _strategy.SearchTarget(this);
    }

    public override void DeactivateNode(NodeEntity node)
    {
        if (_selectedNodes.Contains(node))
        {
            _selectedNodes.Remove(node);
        }
    }

    public override void DeactivateNodes()
    {
        _selectedNodes.Clear();
    }
}