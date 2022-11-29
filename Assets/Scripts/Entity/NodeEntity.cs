using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class NodeEntity : BaseEntity<NodeData>, IUpdatable
{
    private NodeView _nodeView;

    private int _maxUnitCount;
    private int _unitCount;
    
    private int _teamId;
    
    private float _unitReproductionPassTime;
    public float UnitReproductionTimeScale;

    public event Action<int, List<UnitData>, int> UnitsSent;

    public int Id;
    
    public int TeamId
    {
        get => _teamId;
        set
        {
            if (_teamId == value)
                return;
            
            _teamId = value;
            _nodeView.SetSprite(LevelManager.TeamSprites[value]);
        }
    }

    public int UnitCount
    {
        get => _unitCount;
        set
        {
            if (_unitCount == value)
                return;
            
            _unitCount = value;
            _nodeView.SetUnitCountText(value.ToString());
        }
    }

    public Vector3 CurrentPosition => _nodeView.transform.position;
    
    public NodeEntity(NodeData data) : base(data)
    {
        _nodeView = Recycler<NodeView>.Get();
        _nodeView.transform.SetParent(WindowManager.Transform, false);
        _nodeView.transform.position = data.Position - Vector3.forward * 2;
        _nodeView.SetSprite(LevelManager.TeamSprites[data.TeamId]);
        _nodeView.SetRadius(data.Radius);
        _nodeView.SetHighlighted(false);
        _nodeView.NodeEntity = this;

        _teamId = data.TeamId;
        _maxUnitCount = data.MaxUnitCount;
        UnitCount = data.Injection;

        if (LevelManager.IsNetwork)
        {
            UnitsSent += LevelManager.PlayerController.OnUnitsSent;
        }
        else
            UnitsSent += (_, units, target) => SendUnits(units, target);
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
        var unitsToSend = UnitCount / 2;
        var basePos = _nodeView.transform.position;
        var units = new List<UnitData>();
        for (var i = 0; i < unitsToSend; i++)
        {
            var posInCircle = Random.insideUnitCircle * Data.Radius / 100;
            var data = new UnitData
            {
                Position = new Vector3(basePos.x + posInCircle.x, basePos.y + posInCircle.y, basePos.z + 1),
                EndPosition = target._nodeView.transform.position + Vector3.forward,
                TeamId = TeamId,
                Attack = attack,
                Speed = speed
            };
            units.Add(data);
        }
        
        UnitsSent?.Invoke(Id, units, target.Id);
    }

    public void SendUnits(List<UnitData> units, int targetId)
    {
        UnitCount -= UnitCount / 2;
        
        foreach (var u in units)
        {
            var unit = WorldManager.CurrentWorld.CreateNewObject(u) as UnitEntity;
            unit?.Run(targetId);
            _nodeView.transform.DOMove(_nodeView.transform.position + (u.EndPosition - u.Position) * 0.005f, 1f);
        }
    }

    public override void Dispose()
    {
        UnitsSent = null;
        StopHighlighting();
        Recycler<NodeView>.Release(_nodeView.gameObject.GetComponent<NodeView>());
    }
}