using System;

[Serializable]
public class PlayerData : TeamData
{
    public override BaseEntity CreateEntity(IWorld world)
    {
        return new PlayerEntity(world, this);
    }
}