using UnityEngine;
using UnityEngine.UI;

public class GameplayPage : Page
{
    [SerializeField] private Button _pauseButton;
    
    private Gameplay _gameplay;
    
    protected override void OnOpenStart(ViewParam viewParam)
    {
        if (viewParam is not Param param)
        {
            Debug.LogError("GameplayPage: Not Found ViewParam");
            return;
        }
        
        _pauseButton.onClick.AddListener(OnPauseButtonClicked);

        _gameplay = new Gameplay();
        _gameplay.Init(param.LevelIndex);
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