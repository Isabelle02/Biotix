using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : Button
{
    [SerializeField] private GameObject _locked;
    [SerializeField] private GameObject _unlocked;
    [SerializeField] private GameObject _passed;
    [SerializeField] private Text _levelNumber;
    
    private Dictionary<LevelState, GameObject> _states;

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

    public void SetState(LevelState state)
    {
        foreach (var s in _states)
            s.Value.SetActive(s.Key == state);

        _levelNumber.gameObject.SetActive(state != LevelState.Locked);
    }

    public void SetLevelNumber(int lvlNum)
    {
        _levelNumber.text = lvlNum.ToString();
    }
}

public enum LevelState
{
    Locked,
    Unlocked,
    Passed
}