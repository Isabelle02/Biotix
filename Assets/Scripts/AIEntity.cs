using System.Collections.Generic;

public class AIEntity : BaseEntity<AIData>, ITeam
{
    public readonly List<NodeEntity> Nodes = new();

    public AIEntity(IWorld world, AIData data) : base(data)
    {
        foreach (var n in data.NodesData)
        {
            var node = world.CreateNewObject(n) as NodeEntity;
            Nodes.Add(node);
        }
    }
}