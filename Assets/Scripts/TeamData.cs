using System;
using System.Collections.Generic;

[Serializable]
public class TeamData
{
    public int TeamID;
    public List<NodeData> NodesData = new();
}