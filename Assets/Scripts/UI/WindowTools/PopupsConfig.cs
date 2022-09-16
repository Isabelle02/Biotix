using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupsConfig", menuName = "Configs/PopupsConfig")]
public class PopupsConfig : ScriptableObject
{
    [SerializeField] private List<Popup> _popupPrefabs;

    public List<Popup> PopupPrefabs => _popupPrefabs;
}