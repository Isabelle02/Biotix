using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : Window
{
    [SerializeField] private Button _closeButton;
    
    public override void OnOpenStart(ViewParam viewParam)
    {
        _closeButton.onClick.AddListener(OnCLoseButtonClicked);
    }

    private void OnCLoseButtonClicked()
    {
        WindowManager.Open<MainMenuWindow>();
    }

    public override void OnCloseStart()
    {
        _closeButton.onClick.RemoveListener(OnCLoseButtonClicked);
    }
}