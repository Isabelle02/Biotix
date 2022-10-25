using UnityEngine;
using UnityEngine.UI;

public class SettingsPage : Page
{
    [SerializeField] private Button _closeButton;
    
    public override void OnOpenStart(IPage.ViewParam viewParam)
    {
        _closeButton.onClick.AddListener(OnCLoseButtonClicked);
    }

    private void OnCLoseButtonClicked()
    {
        PageManager.Open<MainMenuPage>();
    }

    public override void OnCloseStart()
    {
        _closeButton.onClick.RemoveListener(OnCLoseButtonClicked);
    }
}