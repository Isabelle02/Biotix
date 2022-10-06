using DG.Tweening;
using UnityEngine;

public class NodeEntity : BaseEntity<NodeData>, IUpdatable
{
    private NodeView _nodeView;

    private int _maxUnitCount;
    private int _unitCount;
    
    private int _teamId;
    
    private float _unitReproductionPassTime;
    public float UnitReproductionTimeScale;
    
    public int TeamId
    {
        get => _teamId;
        set
        {
            _teamId = value;
            _nodeView.SetSprite(LevelManager.TeamSprites[value]);
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
        _nodeView.SetSprite(LevelManager.TeamSprites[data.TeamId]);
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

    public void PlayTargetHighlighting()
    {
        _nodeView.PlayTargetHighlighting();
    }

    public void PlayHighlighting()
    {
        _nodeView.PlayHighlighting();
    }

    public void StopHighlighting()
    {
        _nodeView.StopHighlighting();
    }

    public void Update(float delta)
    {
        if (TeamId == 0)
            return;
        
        _unitReproductionPassTime += delta;
        if (_unitReproductionPassTime >= UnitReproductionTimeScale)
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

    public void SendUnits(NodeEntity target, float attack, float speed)
    {
        var unitsToSend = UnitCount - UnitCount / 2;
        UnitCount /= 2;
        var basePos = _nodeView.transform.position;
        for (var i = 0; i < unitsToSend; i++)
        {
            var data = new UnitData();
            var posInCircle = Random.insideUnitCircle * Data.Radius / 100;
            data.Position = new Vector3(basePos.x + posInCircle.x, basePos.y + posInCircle.y, basePos.z + 1);
            data.EndPosition = target._nodeView.transform.position + Vector3.forward;
            data.TeamId = TeamId;
            data.Attack = attack;
            data.Speed = speed;
            var unit = WorldManager.CurrentWorld.CreateNewObject(data) as UnitEntity;
            unit?.Run(target);
            _nodeView.transform.DOMove(_nodeView.transform.position + (data.EndPosition - data.Position) * 0.005f, 1f);
        }
    }

    public override void Dispose()
    {
        Recycler<NodeView>.Release(_nodeView);
    }
}