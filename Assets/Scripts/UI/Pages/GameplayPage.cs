﻿using UnityEngine;
using UnityEngine.UI;

public class GameplayPage : Page
{
    [SerializeField] private Button _pauseButton;
    
    private Gameplay _gameplay;
    
    protected override void OnOpenStart(ViewParam viewParam)
    {
        _pauseButton.onClick.AddListener(OnPauseButtonClicked);

        _gameplay = new Gameplay();
        _gameplay.Init();
    }

    private void OnPauseButtonClicked()
    {
        WorldManager.CurrentWorld.GetSystem<UpdateSystem>()?.SetPause(true);
        WorldManager.CurrentWorld.GetSystem<UnitSystem>()?.SetPause(true);

        _pauseButton.gameObject.SetActive(false);
        PopupManager.Open<PausePopup>();
        PopupManager.Closed += OnPausePopupClosed;
    }

    private void OnPausePopupClosed(Popup popup)
    {
        if (popup is not PausePopup)
            return;
        
        WorldManager.CurrentWorld.GetSystem<UpdateSystem>()?.SetPause(false);
        WorldManager.CurrentWorld.GetSystem<UnitSystem>()?.SetPause(false);
        _pauseButton.gameObject.SetActive(true);
    }

    protected override void OnCloseStart()
    {
        _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);

        var updateSystem = WorldManager.CurrentWorld.GetSystem<UpdateSystem>();
        WorldManager.CurrentWorld.Deactivate();
        updateSystem.ClearUpdatables();
    }

    public class Param : ViewParam
    {
        public readonly int LevelIndex;

        public Param(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }
}