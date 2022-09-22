using UnityEngine;
using UnityEngine.UI;

public class MatchCompletionPopup : Popup
{
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    
    protected override void OnOpenStart(ViewParam viewParam)
    {
        _nextButton.onClick.AddListener(OnNextButtonClicked);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnMainMenuButtonClicked()
    {
        PopupManager.CloseLast();
        PageManager.Open<MainMenuPage>();
    }

    private void OnRestartButtonClicked()
    {
        PopupManager.CloseLast();
        PageManager.Open<GameplayPage>();
    }

    private void OnNextButtonClicked()
    {
        PopupManager.CloseLast();
        PageManager.Open<GameplayPage>();
    }

    protected override void OnCloseStart()
    {
        _nextButton.onClick.RemoveListener(OnNextButtonClicked);
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
    }
}