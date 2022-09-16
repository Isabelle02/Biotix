using System;

[Serializable]
public abstract class BaseData
{
    public abstract BaseEntity CreateEntity(IWorld world);
}