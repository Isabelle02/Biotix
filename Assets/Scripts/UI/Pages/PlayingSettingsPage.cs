using UnityEngine;
using UnityEngine.UI;

public class PlayingSettingsPage : Page
{
    [SerializeField] private Button _singlePlayerButton;
    [SerializeField] private Button _multiplayerButton;
    [SerializeField] private Button _backButton;
    
    public override void OnOpenStart(IPage.ViewParam viewParam)
    {
        _singlePlayerButton.onClick.AddListener(OnSinglePlayerButtonClick);
        _multiplayerButton.onClick.AddListener(OnMultiplayerButtonClick);
        _backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnSinglePlayerButtonClick()
    {
        PageManager.Open<LevelsPage>();
    }

    private void OnMultiplayerButtonClick()
    {
        PageManager.Open<NetworkPlayingConnectionPage>();
    }
    
    private void OnBackButtonClick()
    {
        PageManager.Open<MainMenuPage>();
    }

    public override void OnCloseStart()
    {
        _singlePlayerButton.onClick.RemoveListener(OnSinglePlayerButtonClick);
        _multiplayerButton.onClick.RemoveListener(OnMultiplayerButtonClick);
        _backButton.onClick.RemoveListener(OnBackButtonClick);
    }
}