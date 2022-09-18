using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SplashPage : Page
{
    [SerializeField] private Image _biohazardImage;
    [SerializeField] private float _fadingAnimDuration;

    protected override void OnOpenStart(ViewParam viewParam)
    {
        _biohazardImage.SetAlpha(Constants.Transparent);
        _biohazardImage.DoFade(Constants.Opaque, _fadingAnimDuration).OnComplete(PageManager.CloseLast);
    }

    protected override void OnCloseStart()
    {
        SceneHandler.Load("Main");
        PageManager.Open<MainMenuPage>();
    }
}