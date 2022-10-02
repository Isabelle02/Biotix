using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : Button
{
    [SerializeField] private GameObject _locked;
    [SerializeField] private GameObject _unlocked;
    [SerializeField] private GameObject _passed;
    [SerializeField] private GameObject _comingSoon;
    [SerializeField] private Text _levelNumberText;
    [SerializeField] private int _levelNumber;
    
    private Dictionary<LevelState, GameObject> _states;

    private bool _isComingSoon;

    private bool IsComingSoon
    {
        get => _isComingSoon;
        set
        {
            _isComingSoon = value;
            _comingSoon.SetActive(value);
            
            foreach (var s in _states)
                s.Value.SetActive(!value);

            _levelNumberText.gameObject.SetActive(!value);
        }
    }

    public int LevelNumber => _levelNumber;

    protected override void Awake()
    {
        base.Awake();
        
        _states = new Dictionary<LevelState, GameObject>
        {
            [LevelState.Locked] = _locked,
            [LevelState.Unlocked] = _unlocked, 
            [LevelState.Passed] = _passed
        };
    }

    public void Init()
    {
        IsComingSoon = LevelManager.LevelsCount < _levelNumber;
        
        _levelNumberText.text = _levelNumber.ToString();
        onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (IsComingSoon)
            return;
        
        LevelManager.CurrentLevelIndex = _levelNumber - 1;
        PageManager.Open<GameplayPage>(new GameplayPage.Param(_levelNumber - 1));
    }

    public void SetState(LevelState state)
    {
        if (IsComingSoon)
            return;
        
        foreach (var s in _states)
            s.Value.SetActive(s.Key == state);

        _levelNumberText.gameObject.SetActive(state != LevelState.Locked);
    }

    public void Dispose()
    {
        onClick.RemoveListener(OnClick);
    }
}

public enum LevelState
{
    Locked,
    Unlocked,
    Passed
}