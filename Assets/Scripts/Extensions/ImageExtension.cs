using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class ImageExtension
{
    public static void SetAlpha(this Image image, float alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    public static Tween DoFade(this Image image, float targetAlpha, float duration)
    {
        return DOTween.To(() => image.color.a, image.SetAlpha, targetAlpha, duration);
    }
}