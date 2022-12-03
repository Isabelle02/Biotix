using UnityEngine;
using UnityEngine.UI;

public static class TextExtension
{
    public static void SetAlpha(this Text text, float alpha)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }
}