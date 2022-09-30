using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static LevelsConfig LevelsConfig { get; private set; }
    public static List<LevelStats> LevelStatsMap = new();

    private const string PassedLevelsKey = "PassedLevels";
    private static string _levelStatsMapFIleName;

    public static int PassedLevelsCount
    {
        get => PlayerPrefs.GetInt(PassedLevelsKey, 0);
        set => PlayerPrefs.SetInt(PassedLevelsKey, value);
    }

    public static int CurrentLevelIndex;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
        _levelStatsMapFIleName = Path.Combine(Application.persistentDataPath, "LevelStatsMap");
        
        LevelsConfig = Resources.Load<LevelsConfig>("Configs/LevelsConfig");
        LevelStatsMap = Storage.Load<List<LevelStats>>(_levelStatsMapFIleName);
    }

    public static void SaveLevelStatsMap()
    {
        Storage.Save(_levelStatsMapFIleName, LevelStatsMap);
    }

    public static void ResetLevel()
    {
        _instance.DestroyObjectsOfType<NodeView>();
    }
}