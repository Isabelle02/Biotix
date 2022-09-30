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
    [SerializeField] private Text _awardText;
    
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

        _nextButton.gameObject.SetActive(param.IsWin &&
            LevelManager.CurrentLevelIndex != LevelManager.LevelsConfig.LevelsData.Count - 1);
        
        _nextButton.onClick.AddListener(OnNextButtonClicked);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);

        var levelPassTime = PageManager.Get<GameplayPage>().LevelPassTime;
        
        _timeText.text = $"{levelPassTime.Minute:00} : {levelPassTime.Second:00}";
        _resultText.text = param.IsWin ? "VICTORY" : "DEFEAT";
        
        _newBestTimeText.gameObject.SetActive(false);

        if (!param.IsWin)
            return;

        if (LevelManager.CurrentLevelIndex > LevelManager.LevelStatsMap.Count - 1)
        {
            var levelStats = new LevelStats();
            levelStats.BestTime = levelPassTime;
            LevelManager.LevelStatsMap.Add(levelStats);
            _newBestTimeText.gameObject.SetActive(true);
        }
        else if (levelPassTime < LevelManager.LevelStatsMap[LevelManager.CurrentLevelIndex].BestTime)
        {
            LevelManager.LevelStatsMap[LevelManager.CurrentLevelIndex].BestTime = levelPassTime;
            _newBestTimeText.gameObject.SetActive(true);
        }

        LevelManager.SaveLevelStatsMap();
    }

    private void OnMainMenuButtonClicked()
    {
        PopupManager.CloseLast();
        PageManager.Open<MainMenuPage>();
    }

    private void OnRestartButtonClicked()
    {
        PopupManager.CloseLast();
        PageManager.Open<GameplayPage>(new GameplayPage.Param(LevelManager.CurrentLevelIndex));
    }

    private void OnNextButtonClicked()
    {
        PopupManager.CloseLast();
        PageManager.Open<GameplayPage>(new GameplayPage.Param(LevelManager.CurrentLevelIndex + 1));
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

        public Param(bool isWin)
        {
            IsWin = isWin;
        }
    }
}