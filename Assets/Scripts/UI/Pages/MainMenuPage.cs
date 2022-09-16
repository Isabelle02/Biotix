using UnityEngine;
using UnityEngine.UI;

public class MainMenuPage : Page
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _ratingButton;
    [SerializeField] private Toggle _soundToggle;
    
    protected override void OnOpenStart(ViewParam viewParam)
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _ratingButton.onClick.AddListener(OnRatingButtonClicked);
        _soundToggle.onValueChanged.AddListener(OnSoundCheckBoxValueChanged);
    }

    private void OnSoundCheckBoxValueChanged(bool value)
    {
        
    }

    private void OnPlayButtonClicked()
    {
        PageManager.Open<GameplayPage>();
    }

    private void OnSettingsButtonClicked()
    {
        
    }

    private void OnRatingButtonClicked()
    {
        
    }

    protected override void OnCloseStart()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
        _ratingButton.onClick.RemoveListener(OnRatingButtonClicked);
        _soundToggle.onValueChanged.RemoveListener(OnSoundCheckBoxValueChanged);
    }
}