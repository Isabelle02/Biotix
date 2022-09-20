public class UnitEntity : BaseEntity<UnitData>
{
    private UnitView _unitView;

    public UnitEntity(IWorld world, UnitData data) : base(data)
    {
        _unitView = Recycler<UnitView>.Get();
        _unitView.Disposed += () => world.RemoveEntity(this);
    }
}