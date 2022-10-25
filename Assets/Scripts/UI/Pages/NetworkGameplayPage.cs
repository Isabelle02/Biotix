using System;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGameplayPage : Page
{
    [SerializeField] private Button _pauseButton;

    public override void OnOpenStart(IPage.ViewParam viewParam)
    {
        _pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    private void OnPauseButtonClicked()
    {
        _pauseButton.gameObject.SetActive(false);
        PopupManager.Open<PausePopup>();
        PopupManager.Closed += OnPausePopupClosed;
    }

    private void OnPausePopupClosed(Popup popup)
    {
        if (popup is not PausePopup)
            return;

        _pauseButton.gameObject.SetActive(true);
    }

    public override void OnCloseStart()
    {
        _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);

        var updateSystem = WorldManager.CurrentWorld.GetSystem<UpdateSystem>();
        WorldManager.CurrentWorld.Deactivate();
        updateSystem.ClearUpdatables();

        LevelManager.IsNetwork = false;
    }
}