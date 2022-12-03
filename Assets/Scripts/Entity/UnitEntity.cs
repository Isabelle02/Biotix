using UnityEngine;

public class UnitEntity : BaseEntity<UnitData>, IActor
{
    private UnitView _unitView;

    private Vector3 _endPosition;
    private float _speed;

    public float Attack => Data.Attack;

    public int TeamId
    {
        get => Data.TeamId;
        set => Data.TeamId = value;
    }

    public UnitEntity(UnitData data) : base(data)
    {
        _unitView = Recycler<UnitView>.Get();
        _unitView.transform.SetParent(WindowManager.Transform, false);
        _unitView.transform.position = data.Position;
        _unitView.SetSprite(LevelManager.TeamSprites[data.TeamId]);
        _unitView.UnitEntity = this;

        _endPosition = data.EndPosition;
        _speed = data.Speed;
    }

    public void Run(int targetId)
    {
        _unitView.TargetNodeId = targetId;
        _unitView.Run(_endPosition, _speed);
    }

    public void SetPause(bool isPaused)
    {
        _unitView.SetPause(isPaused);
    }

    public override void Dispose()
    {
        Recycler<UnitView>.Release(_unitView.gameObject.GetComponent<UnitView>());
    }
}