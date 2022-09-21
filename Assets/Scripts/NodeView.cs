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

    public NodeEntity NodeEntity;

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
    }
}