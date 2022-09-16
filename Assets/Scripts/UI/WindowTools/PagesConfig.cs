using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PagesConfig", menuName = "Configs/PagesConfig")]
public class PagesConfig : ScriptableObject
{
    [SerializeField] private List<Page> _pagePrefabs;

    public List<Page> PagePrefabs => _pagePrefabs;
}
