using UnityEngine;
using UnityEngine.UI;

public class PlayingSettingsWindow : Window
{
    [SerializeField] private Button _singlePlayerButton;
    [SerializeField] private Button _multiplayerButton;
    [SerializeField] private Button _backButton;
    
    public override void OnOpenStart(ViewParam viewParam)
    {
        _singlePlayerButton.onClick.AddListener(OnSinglePlayerButtonClick);
        _multiplayerButton.onClick.AddListener(OnMultiplayerButtonClick);
        _backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnSinglePlayerButtonClick()
    {
        WindowManager.Open<LevelsWindow>();
    }

    private void OnMultiplayerButtonClick()
    {
        WindowManager.Open<NetworkPlayingConnectionWindow>();
    }
    
    private void OnBackButtonClick()
    {
        WindowManager.Open<MainMenuWindow>();
    }

    public override void OnCloseStart()
    {
        _singlePlayerButton.onClick.RemoveListener(OnSinglePlayerButtonClick);
        _multiplayerButton.onClick.RemoveListener(OnMultiplayerButtonClick);
        _backButton.onClick.RemoveListener(OnBackButtonClick);
    }
}