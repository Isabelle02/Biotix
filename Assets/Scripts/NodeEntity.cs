using UnityEngine;

public class NodeEntity : BaseEntity<NodeData>, IUpdatable
{
    private NodeView _nodeView;

    private int _maxUnitCount;
    private int _unitCount;

    private float _unitReproductionTimeScale = 1;
    private float _unitReproductionPassTime;

    private int _teamId;
    
    public int TeamId
    {
        get => _teamId;
        set
        {
            _teamId = value;
            _nodeView.SetSprite(LevelManager.LevelsConfig.TeamSprites[value]);
            var system = WorldManager.CurrentWorld.GetSystem<NodeSystem>();
            system.UpdateNodes(this);
        }
    }

    public int UnitCount
    {
        get => _unitCount;
        set
        {
            _unitCount = value;
            _nodeView.SetUnitCountText(value.ToString());
        }
    }

    public Vector3 CurrentPosition => _nodeView.transform.position;
    
    public NodeEntity(NodeData data) : base(data)
    {
        _nodeView = Recycler<NodeView>.Get();
        _nodeView.transform.SetParent(PageManager.Get<GameplayPage>().transform, false);
        _nodeView.transform.position = data.Position - Vector3.forward * 2;
        _nodeView.SetSprite(LevelManager.LevelsConfig.TeamSprites[data.TeamId]);
        _nodeView.SetRadius(data.Radius);
        _nodeView.SetHighlighted(false);
        _nodeView.NodeEntity = this;

        _teamId = data.TeamId;
        _maxUnitCount = data.MaxUnitCount;
        UnitCount = data.Injection;
    }

    public void SetHighlighted(bool isHighlighted)
    {
        _nodeView.SetHighlighted(isHighlighted);
    }

    public void Update(float delta)
    {
        if (TeamId == 0)
            return;
        
        _unitReproductionPassTime += delta;
        if (_unitReproductionPassTime >= _unitReproductionTimeScale)
        {
            if (UnitCount > _maxUnitCount)
                UnitCount--;
            else if (UnitCount < _maxUnitCount)
                UnitCount++;
            
            _unitReproductionPassTime = 0;
        }
    }
    
    public void SetLineEndPosition(Vector3 endPos)
    {
        _nodeView.SetLineEndPosition(endPos);
    }

    public void SetLineActive(bool isActive)
    {
        _nodeView.SetLineActive(isActive);
    }

    public void SendUnits(NodeEntity node)
    {
        var unitsToSend = UnitCount - UnitCount / 2;
        UnitCount /= 2;
        var basePos = _nodeView.transform.position;
        for (var i = 0; i < unitsToSend; i++)
        {
            var data = new UnitData();
            var posInCircle = Random.insideUnitCircle * Data.Radius / 100;
            data.Position = new Vector3(basePos.x + posInCircle.x, basePos.y + posInCircle.y, basePos.z + 1);
            data.EndPosition = data.Position + (node._nodeView.transform.position - basePos);
            data.TeamId = TeamId;
            var unit = WorldManager.CurrentWorld.CreateNewObject(data) as UnitEntity;
            unit?.Run(node);
        }
    }

    public override void Dispose()
    {
        Recycler<NodeView>.Release(_nodeView);
    }
}