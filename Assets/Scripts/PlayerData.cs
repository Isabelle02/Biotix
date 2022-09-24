using UnityEngine;

public class PlayerData : LaboratoryData
{
    private const string PlayerAttackKey = "PLayerAttack";
    private const string PlayerDefenceKey = "PLayerDefence";
    private const string PlayerSpeedKey = "PLayerSpeed";
    private const string PlayerReproductionKey = "PLayerReproduction";
    private const string PlayerAdditionalInjectionKey = "PLayerAdditionalInjection";
    
    public override float Attack
    {
        get => PlayerPrefs.GetFloat(PlayerAttackKey, BaseAttack);
        set => PlayerPrefs.SetFloat(PlayerAttackKey, value);
    }
    
    public override float Defence
    {
        get => PlayerPrefs.GetFloat(PlayerDefenceKey, BaseDefence);
        set => PlayerPrefs.SetFloat(PlayerDefenceKey, value);
    }
    
    public override float Speed
    {
        get => PlayerPrefs.GetFloat(PlayerSpeedKey, BaseSpeed);
        set => PlayerPrefs.SetFloat(PlayerSpeedKey, value);
    }
    
    public override float Reproduction
    {
        get => PlayerPrefs.GetFloat(PlayerReproductionKey, BaseReproduction);
        set => PlayerPrefs.SetFloat(PlayerReproductionKey, value);
    }
    
    public override int AdditionalInjection
    {
        get => PlayerPrefs.GetInt(PlayerAdditionalInjectionKey, BaseAdditionalInjection);
        set => PlayerPrefs.SetInt(PlayerAdditionalInjectionKey, value);
    }
}