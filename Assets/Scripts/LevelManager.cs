using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    
    private static string _levelStatsMapFIleName;
    
    private const string PlayerSpriteIndexKey = "PlayerSpriteIndex";
    private const string PassedLevelsKey = "PassedLevels";
    
    public static List<Sprite> TeamSprites = new();
    public static List<LevelStats> LevelStatsMap = new();

    private static LevelsConfig _levelsConfig;
    
    public static int CurrentLevelIndex;

    public static int LevelsCount => _levelsConfig.LevelsData.Count;
    
    public static int PlayerSpriteIndex
    {
        get => PlayerPrefs.GetInt(PlayerSpriteIndexKey, 1);
        set
        {
            TeamSprites = new List<Sprite>(_levelsConfig.TeamSprites);
            (TeamSprites[1], TeamSprites[value]) = (TeamSprites[value], TeamSprites[1]);
            PlayerPrefs.SetInt(PlayerSpriteIndexKey, value);
        }
    }

    public static int PassedLevelsCount
    {
        get => PlayerPrefs.GetInt(PassedLevelsKey, 0);
        set => PlayerPrefs.SetInt(PassedLevelsKey, value);
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
        _levelStatsMapFIleName = Path.Combine(Application.persistentDataPath, "LevelStatsMap");
        
        _levelsConfig = Resources.Load<LevelsConfig>("Configs/LevelsConfig");
        LevelStatsMap = Storage.Load<List<LevelStats>>(_levelStatsMapFIleName);
        
        TeamSprites = new List<Sprite>(_levelsConfig.TeamSprites);
        (TeamSprites[1], TeamSprites[PlayerSpriteIndex]) = (TeamSprites[PlayerSpriteIndex], TeamSprites[1]);
    }

    public static LevelData GetLevel(int index)
    {
        return _levelsConfig.LevelsData[index];
    }

    public static void SaveLevelStatsMap()
    {
        Storage.Save(_levelStatsMapFIleName, LevelStatsMap);
    }
}