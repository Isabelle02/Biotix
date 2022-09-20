public class UnitData : BaseData
{
    public override BaseEntity CreateEntity(IWorld world)
    {
        return new UnitEntity(world, this);
    }
}