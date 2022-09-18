using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class TextExtension
{
    public static void SetAlpha(this Text text, float alpha)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }

    public static Tween DoFade(this Text text, float targetAlpha, float duration)
    {
        return DOTween.To(() => text.color.a, text.SetAlpha, targetAlpha, duration);
    }
}