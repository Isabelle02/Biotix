using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WindowsInfo", menuName = "Infos/WindowsInfo")]
public class WindowsConfig : ScriptableObject
{
    [SerializeField] private List<Window> windowPrefabs;

    public List<Window> WindowPrefabs => windowPrefabs;
}
