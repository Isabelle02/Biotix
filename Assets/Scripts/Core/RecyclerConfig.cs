using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecyclerConfig", menuName = "Configs/RecyclerConfig")]
public class RecyclerConfig : ScriptableObject
{
    public List<GameObject> Prefabs = new List<GameObject>();
}