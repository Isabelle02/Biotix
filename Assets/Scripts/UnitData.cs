using UnityEngine;

public class UnitData : BaseData
{
    public Vector3 Position;
    public Vector3 EndPosition;
    public float Attack = 1;
    public float Defence = 1;
    public float Speed = 1;
    public int TeamId;
    
    public override BaseEntity CreateEntity(IWorld world)
    {
        return new UnitEntity(world, this);
    }
}