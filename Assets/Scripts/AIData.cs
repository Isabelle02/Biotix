using System;

[Serializable]
public class AIData : TeamData
{
    public override BaseEntity CreateEntity(IWorld world)
    {
        return new AIEntity(world, this);
    }
}