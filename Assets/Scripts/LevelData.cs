using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public List<PlayerData> PlayersData = new();
    public List<AIData> AisData = new();
}