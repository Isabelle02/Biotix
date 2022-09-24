using System.Collections.Generic;
using UnityEngine;

public class LevelsPage : Page
{
    [SerializeField] private List<LevelButton> _levelButtons;

    protected override void OnOpenStart(ViewParam viewParam)
    {
        foreach (var lb in _levelButtons)
        {
            lb.Init();
            
            var state = lb.LevelNumber <= LevelManager.PassedLevelsCount ? 
                LevelState.Passed :
                lb.LevelNumber == LevelManager.PassedLevelsCount + 1 ? 
                    LevelState.Unlocked : 
                    LevelState.Locked;
            lb.SetState(state);
        }
    }

    protected override void OnCloseStart()
    {
        foreach (var lb in _levelButtons)
        {
            lb.Dispose();
        }
    }
}