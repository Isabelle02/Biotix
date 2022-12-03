using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class SceneHandler
{
    public static event Action SceneLoaded;
    
    public static async void Load(string name)
    {
        await SceneManager.LoadSceneAsync(name);
        SceneLoaded?.Invoke();
    }

    public static async void Unload(string name)
    {
        await SceneManager.UnloadSceneAsync(name);
    }
}