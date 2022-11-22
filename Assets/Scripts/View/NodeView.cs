using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NodeView : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CircleCollider2D _circleCollider2D;
    [SerializeField] private Image _image;
    [SerializeField] private Text _unitCountText;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Image _highlightingImage;
    [SerializeField] private Image _targetHighlightingImage;

    private Sequence _highlightingSequence;

    private Vector3 _baseHighlightingImageScale;
    
    public NodeEntity NodeEntity { get; set; }

    private void Awake()
    {
        _highlightingImage.enabled = false;
        _targetHighlightingImage.enabled = false;
        _baseHighlightingImageScale = _highlightingImage.transform.localScale;
    }

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void SetUnitCountText(string count)
    {
        _unitCountText.text = count;
    }

    public void SetRadius(float radius)
    {
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, radius * 2);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, radius * 2);
        _circleCollider2D.radius = radius;
    }

    public void SetLineEndPosition(Vector3 endPos)
    {
        _lineRenderer.SetPositions(new []{transform.position, endPos});
    }

    public void SetLineActive(bool isActive)
    {
        _lineRenderer.enabled = isActive;
    }
    
    public void SetHighlighted(bool isHighlighted)
    {
        _highlightingImage.enabled = isHighlighted;
        if (!isHighlighted)
            return;

        _highlightingImage.transform.DOScale(_baseHighlightingImageScale * 1.1f, 0.15f).SetLoops(2, LoopType.Yoyo);
    }

    public void PlayTargetHighlighting()
    {
        _targetHighlightingImage.enabled = true;
        var baseScale = _targetHighlightingImage.transform.localScale;
        _targetHighlightingImage.transform.DOScale(baseScale * 1.1f, 0.15f).SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => _targetHighlightingImage.enabled = false);
    }

    public void PlayHighlighting()
    {
        _highlightingImage.enabled = true;
        _highlightingImage.SetAlpha(Constants.Opaque);
        _highlightingSequence = DOTween.Sequence()
            .Append(_highlightingImage.transform.DOScale(_baseHighlightingImageScale * 1.2f, 1f))
            .Join(_highlightingImage.DOFade(Constants.Transparent, 1f))
            .SetLoops(-1, LoopType.Restart);
    }

    public void StopHighlighting()
    {
        _highlightingSequence?.Kill();
        _highlightingImage.SetAlpha(Constants.Opaque);
        _highlightingImage.transform.localScale = _baseHighlightingImageScale;
        _highlightingImage.enabled = false;
    }
}