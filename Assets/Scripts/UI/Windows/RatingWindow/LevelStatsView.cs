using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelStatsView : MonoBehaviour
{
    [SerializeField] private Text _levelNumberText;
    [SerializeField] private Text _bestTimeText;

    public void Init(int levelNum, DateTime bestTime)
    {
        _levelNumberText.text = levelNum.ToString();
        _bestTimeText.text = $"{bestTime.Minute:00} : {bestTime.Second:00}";
    }
}