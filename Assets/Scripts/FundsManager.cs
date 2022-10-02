using UnityEngine;

public static class FundsManager
{
    private const string FundsKey = "Funds";
    
    public static int Funds
    {
        get => PlayerPrefs.GetInt(FundsKey, 0);
        private set => PlayerPrefs.SetInt(FundsKey, value);
    }

    public static bool MakeTransaction(int diff)
    {
        if (Funds + diff < 0)
        {
            PopupManager.Open<WarningPopup>(new WarningPopup.Param("Insufficient funds"));
            return false;
        }

        Funds += diff;
        
        return true;
    }
}