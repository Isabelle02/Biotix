using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static LevelsConfig LevelsConfig { get; private set; }

    private const string PassedLevelsKey = "PassedLevels";

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
        
        LevelsConfig = Resources.Load<LevelsConfig>("Configs/LevelsConfig");
    }

    public static void ResetLevel()
    {
        _instance.DestroyObjectsOfType<NodeView>();
    }
}