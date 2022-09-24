using UnityEngine;

public class UnitData : BaseData
{
    public Vector3 Position;
    public Vector3 EndPosition;
    public float Attack;
    public float Speed;
    public int TeamId;
    
    public override BaseEntity CreateEntity(IWorld world)
    {
        return new UnitEntity(this);
    }
}