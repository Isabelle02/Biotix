using UnityEngine;
using UnityEngine.UI;

public class MatchCompletionPopup : Popup
{
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Text _resultText;
    
    protected override void OnOpenStart(ViewParam viewParam)
    {
        if (viewParam is not Param param)
        {
            Debug.LogError("MatchCompletionPopup: Not Found ViewParam");
            return;
        }
        
        WorldManager.CurrentWorld.GetSystem<UpdateSystem>()?.SetPause(true);
        WorldManager.CurrentWorld.GetSystem<UnitSystem>()?.SetPause(true);
        
        _nextButton.onClick.AddListener(OnNextButtonClicked);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);

        _resultText.text = param.IsWin ? "VICTORY" : "DEFEAT";
    }

    private void OnMainMenuButtonClicked()
    {
        PopupManager.CloseLast();
        PageManager.Open<MainMenuPage>();
    }

    private void OnRestartButtonClicked()
    {
        PopupManager.CloseLast();
        PageManager.Open<GameplayPage>();
    }

    private void OnNextButtonClicked()
    {
        PopupManager.CloseLast();
        PageManager.Open<GameplayPage>();
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