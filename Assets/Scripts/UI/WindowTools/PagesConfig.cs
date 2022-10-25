using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PagesConfig", menuName = "Configs/PagesConfig")]
public class PagesConfig : ScriptableObject
{
    [SerializeField] private List<GameObject> _pagePrefabs;

    public List<GameObject> PagePrefabs => _pagePrefabs;
}
