using UnityEngine;
using UnityEngine.UI;

public class GameplayPage : Page
{
    [SerializeField] private Button _pauseButton;
    
    protected override void OnOpenStart(ViewParam viewParam)
    {
        _pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    private void OnPauseButtonClicked()
    {
        
    }

    protected override void OnCloseStart()
    {
        _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
    }
}