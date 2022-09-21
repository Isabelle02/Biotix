using System;
using System.Collections.Generic;

[Serializable]
public class TeamData : BaseData
{
    public int TeamId;
    public List<NodeData> NodesData = new();
    
    public override BaseEntity CreateEntity(IWorld world)
    {
        return default;
    }
}