using System;
using UnityEngine;
using UnityEngine.UI;

public class MatchCompletionPopup : Popup
{
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Text _resultText;
    [SerializeField] private Text _timeText;
    [SerializeField] private Text _newBestTimeText;
    [SerializeField] private Text _rewardText;
    
    protected override void OnOpenStart(ViewParam viewParam)
    {
        if (viewParam is not Param param)
        {
            Debug.LogError("MatchCompletionPopup: Not Found ViewParam");
            return;
        }
        
        WorldManager.CurrentWorld.GetSystem<UpdateSystem>()?.SetPause(true);
        WorldManager.CurrentWorld.GetSystem<UnitSystem>()?.SetPause(true);

        if (param.IsWin && LevelManager.CurrentLevelIndex == LevelManager.PassedLevelsCount)
            LevelManager.PassedLevelsCount++;

        _nextButton.gameObject.SetActive(!LevelManager.IsNetwork && param.IsWin &&
            LevelManager.CurrentLevelIndex != LevelManager.LevelsCount - 1);
        
        _nextButton.onClick.AddListener(OnNextButtonClicked);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        
        _timeText.text = $"{param.LevelPassTime.Minute:00} : {param.LevelPassTime.Second:00}";
        _resultText.text = param.IsWin ? "VICTORY" : "DEFEAT";
        _rewardText.text = param.Reward.ToString("+0;-#");

        _newBestTimeText.gameObject.SetActive(param.IsNewRecord);
    }

    private void OnMainMenuButtonClicked()
    {
        PopupManager.CloseLast();
        WindowManager.Open<MainMenuWindow>();
    }

    private void OnRestartButtonClicked()
    {
        if (LevelManager.IsNetwork)
        {
            PopupManager.CloseLast();
            WindowManager.Open<NetworkPlayingConnectionWindow>();
        }
        else
        {
            PopupManager.CloseLast();
            WindowManager.Open<GameplayWindow>(new GameplayWindow.Param(LevelManager.CurrentLevelIndex));
        }
    }

    private void OnNextButtonClicked()
    {
        PopupManager.CloseLast();
        WindowManager.Open<GameplayWindow>(new GameplayWindow.Param(++LevelManager.CurrentLevelIndex));
    }

    protected override void OnCloseStart()
    {
        _nextButton.onClick.RemoveListener(OnNextButtonClicked);
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
    }

    public class Param : ViewParam
    {
        public readonly bool IsWin;
        public readonly bool IsNewRecord;
        public readonly DateTime LevelPassTime;
        public readonly int Reward;

        public Param(bool isWin, bool isNewRecord, DateTime levelPassTime, int reward)
        {
            IsWin = isWin;
            IsNewRecord = isNewRecord;
            LevelPassTime = levelPassTime;
            Reward = reward;
        }
    }
}