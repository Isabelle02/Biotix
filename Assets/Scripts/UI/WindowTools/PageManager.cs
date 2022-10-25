using System;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] private PagesConfig _pagesConfig;
    
    private static PageManager _instance;
    private readonly Dictionary<Type, IPage> _pagesDictionary = new();
    private readonly Stack<IPage> _pageStack = new();
    private IPage _curPage;

    public static Transform Transform => _instance.transform;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);

        SceneHandler.SceneLoaded += OnSceneLoaded;
        
        Open<SplashPage>();
    }

    private void OnSceneLoaded()
    {
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    public static void Open<T>(IPage.ViewParam viewParam = null) where T : IPage
    {
        if (!_instance._pagesDictionary.ContainsKey(typeof(T)))
        {
            var pagePrefab = _instance._pagesConfig.PagePrefabs.Find(w => w.TryGetComponent<T>(out _));
            if (pagePrefab == null)
                return;

            var page = Instantiate(pagePrefab, _instance.transform);
            page.SetActive(false);
            _instance._pagesDictionary.Add(typeof(T), page.GetComponent<IPage>());
        }
        
        var p = _instance._pagesDictionary[typeof(T)];
        if (_instance._curPage is {IsActive: true})
            _instance._curPage.Close();

        p.Open(viewParam);
        _instance._curPage = p;
        _instance._pageStack.Push(p);
    }

    public static T Get<T>() where T : IPage
    {
        if (_instance._pagesDictionary[typeof(T)] is T page)
            return page;

        return default;
    }
}