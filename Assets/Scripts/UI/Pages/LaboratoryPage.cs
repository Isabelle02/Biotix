using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaboratoryPage : Page
{
    [SerializeField] private VerticalLayoutGroup _featuresList;
    [SerializeField] private Button _backButton;

    private List<FeatureItem> _featureItems = new();

    protected override void OnOpenStart(ViewParam viewParam)
    {
        foreach (var n in Enum.GetValues(typeof(FeatureItemName.FeatureName)))
        {
            var item = Recycler<FeatureItem>.Get();
            item.transform.SetParent(_featuresList.transform, false);
            item.Init(FeatureItemName.GetString((FeatureItemName.FeatureName) n));
            _featureItems.Add(item);
        }
        
        _featuresList.SetLayoutVertical();
        
        _backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnBackButtonClick()
    {
        PageManager.Open<MainMenuPage>();
    }

    protected override void OnCloseStart()
    {
        foreach (var f in _featureItems)
        {
            Recycler<FeatureItem>.Release(f);
        }
        
        _backButton.onClick.RemoveListener(OnBackButtonClick);
    }
}