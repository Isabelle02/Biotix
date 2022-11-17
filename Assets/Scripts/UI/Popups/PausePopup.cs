using System.Threading.Tasks;
using Photon.Pun;
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
        if (!LevelManager.IsNetwork)
        {
            WorldManager.CurrentWorld.GetSystem<UpdateSystem>()?.SetPause(true);
            WorldManager.CurrentWorld.GetSystem<UnitSystem>()?.SetPause(true);
        }

        _soundToggle.isOn = !SoundManager.IsOn;
        
        _restartButton.gameObject.SetActive(!LevelManager.IsNetwork);
        
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
        PageManager.Open<GameplayPage>(new GameplayPage.Param(LevelManager.CurrentLevelIndex));
    }

    private void OnMainMenuButtonClicked()
    {
        if (LevelManager.IsNetwork)
        {
            PhotonNetwork.LeaveRoom();
            while (PhotonNetwork.InRoom)
            {
            }
        }
        
        PopupManager.CloseLast();
        PageManager.Open<MainMenuPage>();
    }

    private async void OnSoundCheckBoxValueChanged(bool value)
    {
        SoundManager.IsOn = true;
        SoundManager.PlayOneShot(Sound.Toggle);
        await Task.Delay((int) (SoundManager.GetClipLength(Sound.Toggle) * 1000));
        SoundManager.IsOn = !value;
    }

    protected override void OnCloseStart()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
        _soundToggle.onValueChanged.RemoveListener(OnSoundCheckBoxValueChanged);

        if (!LevelManager.IsNetwork)
        {
            WorldManager.CurrentWorld.GetSystem<UpdateSystem>()?.SetPause(false);
            WorldManager.CurrentWorld.GetSystem<UnitSystem>()?.SetPause(false);
        }
    }
}