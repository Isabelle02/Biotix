using System;
using UnityEngine;

public static class FundsManager
{
    private const string FundsKey = "Funds";

    public static event Action<int> UpdatedFunds; 

    public static int Funds
    {
        get => PlayerPrefs.GetInt(FundsKey, 0);
        private set
        {
            PlayerPrefs.SetInt(FundsKey, value);
            UpdatedFunds?.Invoke(value);
        }
    }

    public static bool MakeTransaction(int diff)
    {
        if (Funds + diff < 0)
        {
            PopupManager.Open<WarningPopup>(new WarningPopup.Param($"Insufficient funds: {Funds} {diff:+0;-#}"));
            return false;
        }

        Funds += diff;
        
        return true;
    }

    public static int CalculateReward(int levelNum, int levelCompletionTime, bool isWin, bool isFirstWin, bool isNewBestTime)
    {
        var reward = 1;
        if (!isWin) 
            return reward;

        if (isFirstWin)
            reward = Math.Clamp(levelNum * 100 / levelCompletionTime, 10 * levelNum, 100 * levelNum);
        else
            reward = 3 * levelNum;
        
        if (isNewBestTime)
            reward += (int) (reward * 0.05f);

        return reward;
    }

    public static int CalculateNetworkReward(int playerCount, int levelCompletionTime, bool isWin)
    {
        var reward = 1;
        if (!isWin) 
            return reward;

        reward = Math.Clamp(playerCount * 100 / levelCompletionTime, 10 * playerCount, 100 * playerCount);

        return reward;
    }
}