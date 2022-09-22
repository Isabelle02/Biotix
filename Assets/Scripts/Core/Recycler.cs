using System.Collections.Generic;
using UnityEngine;

public class Recycler : MonoBehaviour
{
    [SerializeField] protected RecyclerConfig _recyclerConfig;

    private static Recycler _instance;

    protected static RecyclerConfig RecyclerConfig => _instance._recyclerConfig;
    
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }
}

public class Recycler<T> : Recycler where T : MonoBehaviour
{
    private static readonly Stack<T> _poolObjects = new Stack<T>();

    public static T Get()
    {
        var obj = default(T);
        if (_poolObjects.Count == 0)
            obj = CreateObject();
        else
            obj = _poolObjects.Pop();
        
        obj.gameObject.SetActive(true);
        return obj;
    }

    public static void Release(T obj)
    {
        if (obj is IReleasable releasable)
            releasable.Dispose();
        
        obj.gameObject.SetActive(false);
        _poolObjects.Push(obj);
    }
        
    private static T CreateObject()
    {
        foreach (var p in RecyclerConfig.Prefabs)
        {
            if (!p.TryGetComponent<T>(out var obj)) 
                continue;
            
            var newObject = Instantiate(obj);
            return newObject;
        }

        return default(T);
    }
}