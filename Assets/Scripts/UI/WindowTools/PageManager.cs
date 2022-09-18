using System;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] private PagesConfig _pagesConfig;
    
    private static PageManager _instance;
    private readonly Dictionary<Type, Page> _pagesDictionary = new();
    private readonly Stack<Page> _pageStack = new();
    private Page _curPage;

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

    public static void Open<T>() where T : Page
    {
        if (!_instance._pagesDictionary.ContainsKey(typeof(T)))
        {
            var page = _instance._pagesConfig.PagePrefabs.Find(w => w.GetType() == typeof(T));
            if (page == null)
                return;
            
            var pagePrefab = Instantiate(page, _instance.transform);
            pagePrefab.gameObject.SetActive(false);
            _instance._pagesDictionary.Add(pagePrefab.GetType(), pagePrefab);
        }
        
        var p = _instance._pagesDictionary[typeof(T)];
        if (_instance._curPage != null && _instance._curPage.gameObject.activeSelf)
            _instance._curPage.Close();

        p.Open();
        _instance._curPage = p;
        _instance._pageStack.Push(p);
    }

    public static void CloseLast()
    {
        if (_instance._pageStack.Count == 1)
        {
            var closing = _instance._pageStack.Peek();
            closing.Close();
            return;
        }

        var last = _instance._pageStack.Pop();
        last.Close();
        
        var prev = _instance._pageStack.Peek();
        _instance._pageStack.Push(last);
        prev.Open();
        _instance._curPage = prev;
        _instance._pageStack.Push(prev);
    }
}