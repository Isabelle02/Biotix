using UnityEngine;

public static class PlayerLaboratoryManager
{
    private const string AttackKey = "PlayerAttack";
    private const string DefenceKey = "PlayerDefence";
    private const string SpeedKey = "PlayerSpeed";
    private const string ReproductionKey = "PlayerReproduction";
    private const string AdditionalInjectionKey = "PlayerAdditionalInjection";
    
    public static float AttackRate
    {
        get => PlayerPrefs.GetFloat(AttackKey, 0);
        set => PlayerPrefs.SetFloat(AttackKey, value);
    }

    public static float DefenceRate
    {
        get => PlayerPrefs.GetFloat(DefenceKey, 0);
        set => PlayerPrefs.SetFloat(DefenceKey, value);
    }
    
    public static float SpeedRate
    {
        get => PlayerPrefs.GetFloat(SpeedKey, 0);
        set => PlayerPrefs.SetFloat(SpeedKey, value);
    }

    public static float ReproductionRate
    {
        get => PlayerPrefs.GetFloat(ReproductionKey, 0);
        set => PlayerPrefs.SetFloat(ReproductionKey, value);
    }

    public static float AdditionalInjectionRate
    {
        get => PlayerPrefs.GetFloat(AdditionalInjectionKey, 0);
        set => PlayerPrefs.SetFloat(AdditionalInjectionKey, value);
    }

}