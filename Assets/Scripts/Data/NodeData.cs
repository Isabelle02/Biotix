using System;
using UnityEngine;

[Serializable]
public class NodeData : BaseData
{
    public int TeamId;
    public Vector3 Position;
    public float Radius;
    public int MaxUnitCount;
    public int Injection;
    
    public override BaseEntity CreateEntity(IWorld world)
    {
        return new NodeEntity(this);
    }
}