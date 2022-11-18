using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : Window
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _ratingButton;
    [SerializeField] private Toggle _soundToggle;
    
    public override void OnOpenStart(ViewParam viewParam)
    {
        SoundManager.Play(Sound.Space);
        
        _soundToggle.isOn = !SoundManager.IsOn;
        
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _ratingButton.onClick.AddListener(OnRatingButtonClicked);
        _soundToggle.onValueChanged.AddListener(OnSoundCheckBoxValueChanged);
    }

    private async void OnSoundCheckBoxValueChanged(bool value)
    {
        SoundManager.IsOn = true;
        SoundManager.PlayOneShot(Sound.Toggle);
        await Task.Delay((int) (SoundManager.GetClipLength(Sound.Toggle) * 1000));
        SoundManager.IsOn = !value;
    }

    private void OnPlayButtonClicked()
    {
        WindowManager.Open<PlayingSettingsWindow>();
    }

    private void OnSettingsButtonClicked()
    {
        WindowManager.Open<LaboratoryWindow>();
    }

    private void OnRatingButtonClicked()
    {
        WindowManager.Open<RatingWindow>();
    }

    public override void OnCloseStart()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
        _ratingButton.onClick.RemoveListener(OnRatingButtonClicked);
        _soundToggle.onValueChanged.RemoveListener(OnSoundCheckBoxValueChanged);
    }
}