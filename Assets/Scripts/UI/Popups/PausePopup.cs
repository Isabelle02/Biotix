using UnityEngine;
using UnityEngine.UI;

public class PausePopup : Popup
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Toggle _soundToggle;
    
    protected override void OnOpenStart(ViewParam viewParam)
    {
        WorldManager.CurrentWorld.GetSystem<UpdateSystem>()?.SetPause(true);
        WorldManager.CurrentWorld.GetSystem<UnitSystem>()?.SetPause(true);
        
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        _soundToggle.onValueChanged.AddListener(OnSoundCheckBoxValueChanged);
    }

    private void OnPlayButtonClicked()
    {
        PopupManager.CloseLast();
    }

    private void OnRestartButtonClicked()
    {
        PopupManager.CloseLast();
        PageManager.Open<GameplayPage>();
    }

    private void OnMainMenuButtonClicked()
    {
        PopupManager.CloseLast();
        PageManager.Open<MainMenuPage>();
    }

    private void OnSoundCheckBoxValueChanged(bool value)
    {
        
    }

    protected override void OnCloseStart()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
        _soundToggle.onValueChanged.RemoveListener(OnSoundCheckBoxValueChanged);
        
        WorldManager.CurrentWorld.GetSystem<UpdateSystem>()?.SetPause(false);
        WorldManager.CurrentWorld.GetSystem<UnitSystem>()?.SetPause(false);
    }
}