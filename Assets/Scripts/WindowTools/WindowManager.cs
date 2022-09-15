using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private WindowsConfig _windowsConfig;
    
    private static WindowManager _instance;
    private readonly Dictionary<Type, Window> _windowsDictionary = new();
    private Window _curWind;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    public static void Open<T>() where T : Window
    {
        if (!_instance._windowsDictionary.ContainsKey(typeof(T)))
        {
            var window = _instance._windowsConfig.WindowPrefabs.Find(w => w.GetType() == typeof(T));
            if (window == null)
                return;
            
            var wind = Instantiate(window, _instance.transform);
            wind.gameObject.SetActive(false);
            _instance._windowsDictionary.Add(wind.GetType(), wind);
        }
        
        var w = _instance._windowsDictionary[typeof(T)];
        if (_instance._curWind != null && _instance._curWind.gameObject.activeSelf)
            _instance._curWind.Close();

        w.Open();
        _instance._curWind = w;
    }
}