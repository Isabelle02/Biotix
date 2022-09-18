using UnityEngine;
using UnityEngine.UI;

public class NodeView : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CircleCollider2D _circleCollider2D;
    [SerializeField] private Image _image;
    [SerializeField] private Text _unitCountText;

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
}