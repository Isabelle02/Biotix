using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    private static LevelsConfig _levelsConfig;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
        
        _levelsConfig = Resources.Load<LevelsConfig>("Configs/LevelsConfig");
    }

    public static void ResetLevel()
    {
        DestroyObjectsOfType<NodeView>();
    }

    private static void DestroyObjectsOfType<T>() where T : MonoBehaviour
    {
        var objects = FindObjectsOfType<T>();
        for (var i = objects.Length - 1; i >= 0; i--)
            DestroyImmediate(objects[i].gameObject);
    }
}