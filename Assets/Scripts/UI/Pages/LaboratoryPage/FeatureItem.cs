using System;
using UnityEngine;
using UnityEngine.UI;

public class FeatureItem : MonoBehaviour, IReleasable
{
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _costText;
    [SerializeField] private int _costStep;
    [SerializeField, Range(0, 1)] private float _progressStep = 0.1f;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Image _progressImage;

    private FeatureItemName.FeatureName _featureName;
    
    private string _progressRateKey;
    private string _costKey;

    public float ProgressRate
    {
        get => PlayerPrefs.GetFloat(_progressRateKey, 0);
        private set
        {
            if (value > 1.05f)
                return;
            
            _progressImage.fillAmount = value;
            Cost += _costStep;
            PlayerPrefs.SetFloat(_progressRateKey, _progressImage.fillAmount);
        }
    }

    public int Cost
    {
        get => PlayerPrefs.GetInt(_costKey, 500);
        private set
        {
            _costText.text = value.ToString();
            PlayerPrefs.SetInt(_costKey, value);
        }
    }

    public void Init(FeatureItemName.FeatureName featureName)
    {
        _featureName = featureName;
        
        _progressRateKey = $"{featureName}ProgressRate";
        _costKey = $"{featureName}CostKey";

        _titleText.text = FeatureItemName.GetString(featureName);
        _costText.text = Cost.ToString();
        _progressImage.fillAmount = ProgressRate;
        
        _buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    private void OnBuyButtonClick()
    {
        ProgressRate += _progressStep;
        
        switch (_featureName)
        {
            case FeatureItemName.FeatureName.Attack:
                PlayerLaboratoryManager.AttackRate += ProgressRate;
                break;
            case FeatureItemName.FeatureName.Defence:
                PlayerLaboratoryManager.DefenceRate += ProgressRate;
                break;
            case FeatureItemName.FeatureName.Speed:
                PlayerLaboratoryManager.SpeedRate += ProgressRate;
                break;
            case FeatureItemName.FeatureName.Reproduction:
                PlayerLaboratoryManager.ReproductionRate += ProgressRate;
                break;
            case FeatureItemName.FeatureName.Injection:
                PlayerLaboratoryManager.AdditionalInjectionRate = ProgressRate;
                break;
        }
    }

    public void Dispose()
    {
        _buyButton.onClick.RemoveListener(OnBuyButtonClick);
    }
}

public static class FeatureItemName
{
    public enum FeatureName
    {
        Attack,
        Defence,
        Speed,
        Reproduction,
        Injection
    }

    public static string GetString(FeatureName name)
    {
        return name switch
        {
            FeatureName.Attack => "Attack",
            FeatureName.Defence => "Defence",
            FeatureName.Speed => "Speed",
            FeatureName.Reproduction => "Reproduction",
            FeatureName.Injection => "Injection",
            _ => ""
        };
    }
}