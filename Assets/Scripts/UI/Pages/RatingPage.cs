using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingPage : Page
{
    [SerializeField] private VerticalLayoutGroup _statsList;
    [SerializeField] private Button _backButton;

    private readonly List<LevelStatsView> _levelStatsViews = new();

    public override void OnOpenStart(IPage.ViewParam viewParam)
    {
        _backButton.onClick.AddListener(OnBackButtonClick);
        
        for (var i = 0; i < LevelManager.LevelStatsMap.Count; i++)
        {
            var levelStats = Recycler<LevelStatsView>.Get();
            levelStats.transform.SetParent(_statsList.transform, false);
            levelStats.Init(i + 1, LevelManager.LevelStatsMap[i].BestTime);
            _levelStatsViews.Add(levelStats);
        }
    }

    private void OnBackButtonClick()
    {
        PageManager.Open<MainMenuPage>();
    }

    public override void OnCloseStart()
    {
        foreach (var l in _levelStatsViews)
        {
            Recycler<LevelStatsView>.Release(l);
        }
        
        _backButton.onClick.RemoveListener(OnBackButtonClick);
    }
}