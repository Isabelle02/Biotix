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
        _pauseButton.gameObject.SetActive(false);
        PopupManager.Open<PausePopup>();
        PopupManager.Closed += OnPausePopupClosed;
    }

    private void OnPausePopupClosed(Popup popup)
    {
        if (popup is not PausePopup)
            return;
        
        _pauseButton.gameObject.SetActive(true);
    }

    protected override void OnCloseStart()
    {
        _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
    }
}