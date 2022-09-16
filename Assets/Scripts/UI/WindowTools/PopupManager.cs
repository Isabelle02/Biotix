using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private PopupsConfig _popupsConfig;

    private static PopupManager _instance;
    private readonly Dictionary<Type, Popup> _popupsDictionary = new();
    private Stack<Popup> _openedPopups = new();

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    public static void Open<T>() where T : Popup
    {
        if (!_instance._popupsDictionary.ContainsKey(typeof(T)))
        {
            var popup = _instance._popupsConfig.PopupPrefabs.Find(w => w.GetType() == typeof(T));
            if (popup == null)
                return;

            var popupPrefab = Instantiate(popup, _instance.transform);
            popupPrefab.gameObject.SetActive(false);
            _instance._popupsDictionary.Add(popupPrefab.GetType(), popupPrefab);
        }

        var p = _instance._popupsDictionary[typeof(T)];

        p.Open();
        _instance._openedPopups.Push(p);
    }

    public static void CloseLast()
    {
        var last = _instance._openedPopups.Pop();
        last.Close();
    }

    public static void CloseAll()
    {
        var count = _instance._openedPopups.Count;
        for (var i = 0; i < count; i++)
            CloseLast();
    }
}