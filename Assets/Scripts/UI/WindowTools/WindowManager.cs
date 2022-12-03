using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private WindowsConfig _windowsConfig;
    
    private static WindowManager _instance;
    private readonly Dictionary<Type, Window> _windowsDictionary = new();
    private readonly Stack<Window> _windowStack = new();
    private Window _curWindow;

    public static Transform Transform => _instance.transform;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);

        SceneHandler.SceneLoaded += OnSceneLoaded;
        
        Open<SplashWindow>();
    }

    private void OnSceneLoaded()
    {
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    public static void Open<T>(Window.ViewParam viewParam = null) where T : Window
    {
        if (!_instance._windowsDictionary.ContainsKey(typeof(T)))
        {
            var pagePrefab = _instance._windowsConfig.WindowPrefabs.Find(w => w.TryGetComponent<T>(out _));
            if (pagePrefab == null)
                return;

            var page = Instantiate(pagePrefab, _instance.transform);
            page.gameObject.SetActive(false);
            _instance._windowsDictionary.Add(typeof(T), page);
        }
        
        var p = _instance._windowsDictionary[typeof(T)];
        if (_instance._curWindow is {IsActive: true})
            _instance._curWindow.Close();

        p.Open(viewParam);
        _instance._curWindow = p;
        _instance._windowStack.Push(p);
    }

    public static T Get<T>() where T : Window
    {
        if (_instance._windowsDictionary[typeof(T)] is T page)
            return page;

        return default;
    }
}