using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WindowsConfig", menuName = "Configs/WindowsConfig")]
public class WindowsConfig : ScriptableObject
{
    [SerializeField] private List<Window> _windowPrefabs;

    public List<Window> WindowPrefabs => _windowPrefabs;
}
