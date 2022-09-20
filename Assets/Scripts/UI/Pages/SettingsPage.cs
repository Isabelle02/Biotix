using UnityEngine;
using UnityEngine.UI;

public class SettingsPage : Page
{
    [SerializeField] private Button _closeButton;
    
    protected override void OnOpenStart(ViewParam viewParam)
    {
        _closeButton.onClick.AddListener(OnCLoseButtonClicked);
    }

    private void OnCLoseButtonClicked()
    {
        PageManager.Open<MainMenuPage>();
    }

    protected override void OnCloseStart()
    {
        _closeButton.onClick.RemoveListener(OnCLoseButtonClicked);
    }
}