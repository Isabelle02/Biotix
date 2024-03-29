﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaboratoryWindow : Window
{
    [SerializeField] private Text _fundsText;
    [SerializeField] private VerticalLayoutGroup _featuresList;
    [SerializeField] private Image _teamImage;
    [SerializeField] private Button _teamSpriteButton;
    [SerializeField] private Button _backButton;

    private readonly List<FeatureItem> _featureItems = new();

    public override void OnOpenStart(ViewParam viewParam)
    {
        FundsManager.UpdatedFunds += OnFundsUpdated;
        
        _fundsText.text = FundsManager.Funds.ToString();
        
        foreach (var n in Enum.GetValues(typeof(FeatureItemName.FeatureName)))
        {
            var item = Recycler<FeatureItem>.Get();
            item.transform.SetParent(_featuresList.transform, false);
            item.Init((FeatureItemName.FeatureName) n);
            _featureItems.Add(item);
        }
        
        _featuresList.SetLayoutVertical();

        _teamImage.sprite = LevelManager.TeamSprites[1];
        
        _teamSpriteButton.onClick.AddListener(OnTeamSpriteButtonClick);
        _backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnFundsUpdated(int value)
    {
        _fundsText.text = value.ToString();
    }

    private void OnTeamSpriteButtonClick()
    {
        if (LevelManager.PlayerSpriteIndex < LevelManager.TeamSprites.Count - 1)
            LevelManager.PlayerSpriteIndex++;
        else
            LevelManager.PlayerSpriteIndex = 1;

        _teamImage.sprite = LevelManager.TeamSprites[1];
    }

    private void OnBackButtonClick()
    {
        WindowManager.Open<MainMenuWindow>();
    }

    public override void OnCloseStart()
    {
        FundsManager.UpdatedFunds -= OnFundsUpdated;
        
        foreach (var f in _featureItems)
        {
            Recycler<FeatureItem>.Release(f);
        }
        
        _teamSpriteButton.onClick.RemoveListener(OnTeamSpriteButtonClick);
        _backButton.onClick.RemoveListener(OnBackButtonClick);
    }
}