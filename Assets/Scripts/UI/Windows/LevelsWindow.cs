using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsWindow : Window
{
    [SerializeField] private List<LevelButton> _levelButtons;
    [SerializeField] private Button _backButton;

    public override void OnOpenStart(ViewParam viewParam)
    {
        _backButton.onClick.AddListener(OnBackButtonClick);
        
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

    private void OnBackButtonClick()
    {
        WindowManager.Open<MainMenuWindow>();
    }

    public override void OnCloseStart()
    {
        foreach (var lb in _levelButtons)
        {
            lb.Dispose();
        }
        
        _backButton.onClick.RemoveListener(OnBackButtonClick);
    }
}