using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SplashPage : Page
{
    [SerializeField] private Image _biohazardImage;
    [SerializeField] private float _fadingAnimDuration;

    protected override void OnOpenStart(ViewParam viewParam)
    {
        SceneHandler.SceneLoaded += OnSceneLoaded;
        _biohazardImage.SetAlpha(Constants.Transparent);
        _biohazardImage.DOFade(Constants.Opaque, _fadingAnimDuration).OnComplete(() => SceneHandler.Load("Main"));
    }

    private void OnSceneLoaded()
    {
        PageManager.Open<MainMenuPage>();
    }

    protected override void OnCloseStart()
    {
        SceneHandler.SceneLoaded -= OnSceneLoaded;
    }
}