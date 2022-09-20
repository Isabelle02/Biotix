using UnityEngine;

public class NodeEntity : BaseEntity<NodeData>, IUpdatable
{
    private NodeView _nodeView;

    private int _maxUnitCount;
    private int _unitCount;

    private float _unitReproductionTimeScale = 1;
    private float _unitReproductionPassTime;

    public int UnitCount
    {
        get => _unitCount;
        set
        {
            _unitCount = value;
            _nodeView.SetUnitCountText(value.ToString());
        }
    }
    
    public NodeEntity(NodeData data) : base(data)
    {
        _nodeView = Recycler<NodeView>.Get();
        _nodeView.transform.SetParent(PageManager.Get<GameplayPage>().transform, false);
        _nodeView.transform.position = data.Position;
        _nodeView.SetSprite(LevelManager.LevelsConfig.NodeSprites[data.TeamID]);
        _nodeView.SetRadius(data.Radius);
        _nodeView.SetHighlighted(false);
        _nodeView.NodeEntity = this;
        
        _maxUnitCount = data.MaxUnitCount;
        UnitCount = data.MaxUnitCount;
    }

    public void SetHighlighted(bool isHighlighted)
    {
        _nodeView.SetHighlighted(isHighlighted);
    }

    public void Update(float delta)
    {
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

    public void SendUnits()
    {
        UnitCount /= 2;
    }
}