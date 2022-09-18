using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsConfig", menuName = "Configs/LevelsConfig")]
public class LevelsConfig : ScriptableObject
{
    public NodeView NodeView;
    public List<Sprite> NodeSprites = new();
    public List<LevelData> LevelsData = new();
}