using UnityEngine;

public class UnitEntity : BaseEntity<UnitData>, IActor
{
    private UnitView _unitView;

    private Vector3 _endPosition;
    private float _attack;
    private float _defence;
    private float _speed;

    public int TeamId
    {
        get => Data.TeamId;
        set => Data.TeamId = value;
    }

    public UnitEntity(IWorld world, UnitData data) : base(data)
    {
        _unitView = Recycler<UnitView>.Get();
        _unitView.transform.SetParent(PageManager.Get<GameplayPage>().transform, false);
        _unitView.transform.position = data.Position;
        _unitView.SetSprite(LevelManager.LevelsConfig.TeamSprites[data.TeamId]);
        _unitView.UnitEntity = this;

        _endPosition = data.EndPosition;
        _attack = data.Attack;
        _defence = data.Defence;
        _speed = data.Speed;
    }

    public void Run(NodeEntity target)
    {
        _unitView.TargetNode = target;
        _unitView.Run(_endPosition, _speed);
    }

    public void SetPause(bool isPaused)
    {
        _unitView.SetPause(isPaused);
    }

    public override void Dispose()
    {
        Recycler<UnitView>.Release(_unitView);
    }
}