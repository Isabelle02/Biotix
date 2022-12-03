using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeGroup : MonoBehaviour
{
    [SerializeField] private List<Image> _images;
    [SerializeField] private List<Text> _texts;
    [SerializeField] private List<FadeGroup> _fadeGroups;

    public void SetAlpha(float alpha)
    {
        foreach (var i in _images)
        {
            i.SetAlpha(alpha);
        }

        foreach (var t in _texts)
        {
            t.SetAlpha(alpha);
        }

        foreach (var g in _fadeGroups)
        {
            g.SetAlpha(alpha);
        }
    }
    
    public Tween DoFade(float alpha, float duration)
    {
        var sequence = DOTween.Sequence();
        
        for (var i = 0; i < _images.Count; i++)
        {
            var image = _images[i];

            if (i == 0)
                sequence.Append(image.DOFade(alpha, duration));
            else
                sequence.Join(image.DOFade(alpha, duration));
        }

        for (var i = 0; i < _texts.Count; i++)
        {
            var t = _texts[i];

            if (i == 0 && _images.Count == 0)
                sequence.Append(t.DOFade(alpha, duration));
            else
                sequence.Join(t.DOFade(alpha, duration));
        }

        for (var i = 0; i < _fadeGroups.Count; i++)
        {
            var g = _fadeGroups[i];
            g.DoFade(alpha, duration);

            if (i == 0 && _texts.Count == 0)
                sequence.Append(g.DoFade(alpha, duration));
            else
                sequence.Join(g.DoFade(alpha, duration));
        }

        return sequence;
    }
}