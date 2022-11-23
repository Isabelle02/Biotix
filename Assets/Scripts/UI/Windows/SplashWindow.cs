using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SplashWindow : Window
{
    [SerializeField] private Image _biohazardImage;
    [SerializeField] private float _fadingAnimDuration;

    public override void OnOpenStart(ViewParam viewParam)
    {
        SceneHandler.SceneLoaded += OnSceneLoaded;
        _biohazardImage.SetAlpha(Constants.Transparent);
        _biohazardImage.DOFade(Constants.Opaque, _fadingAnimDuration).OnComplete(() => SceneHandler.Load("Main"));
    }

    private void OnSceneLoaded()
    {
        SoundManager.Play(Sound.Space);
        WindowManager.Open<MainMenuWindow>();
    }

    public override void OnCloseStart()
    {
        SceneHandler.SceneLoaded -= OnSceneLoaded;
    }
}