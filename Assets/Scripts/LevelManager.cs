using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static LevelsConfig LevelsConfig { get; private set; }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
        
        LevelsConfig = Resources.Load<LevelsConfig>("Configs/LevelsConfig");
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