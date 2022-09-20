using System.Collections.Generic;

public class PlayerEntity : BaseEntity<PlayerData>, ITeam
{
    public readonly List<NodeEntity> Nodes = new();
    public readonly List<NodeEntity> SelectedNodes = new();

    public PlayerEntity(IWorld world, PlayerData data) : base(data)
    {
        foreach (var n in data.NodesData)
        {
            var node = world.CreateNewObject(n) as NodeEntity;
            Nodes.Add(node);
        }
    }
}