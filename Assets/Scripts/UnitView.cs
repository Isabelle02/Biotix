using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UnitView : MonoBehaviour, IReleasable
{
    [SerializeField] private Image _teamImage;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private int _minSize;
    [SerializeField] private int _maxSize;
    [SerializeField] private float _moveAnimDuration;
    
    private Tween _runningTween;
    private Tween _moveAnimTween;

    public UnitEntity UnitEntity;

    public NodeEntity TargetNode;
    
    public event Action Disposed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<NodeView>(out var nodeView))
        {
            if (TargetNode == nodeView.NodeEntity)
            {
                if (TargetNode.TeamId == UnitEntity.TeamId || TargetNode.UnitCount == 0)
                {
                    TargetNode.UnitCount++;
                    TargetNode.TeamId = UnitEntity.TeamId;
                }
                else
                {
                    TargetNode.UnitCount--;
                    if (TargetNode.UnitCount == 0)
                        TargetNode.TeamId = 0;
                }
                
                Recycler<UnitView>.Release(this);
            }
        }
    }

    public void SetSprite(Sprite sprite)
    {
        _teamImage.sprite = sprite;
    }

    public void Run(Vector3 endPos, float speed)
    {
        var vector = endPos - transform.position;

        var angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        var duration = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y) / speed;
        _runningTween = transform.DOMove(endPos, duration);
        _moveAnimTween = PlayMoveAnim();
    }

    private Tween PlayMoveAnim()
    {
        return DOTween.To(() => _minSize,
            x => _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x),
            _maxSize,
            _moveAnimDuration).SetLoops(-1, LoopType.Yoyo);
    }
    
    public void Dispose()
    {
        _runningTween?.Kill();
        _moveAnimTween?.Kill();
        Disposed?.Invoke();
        Disposed = null;
    }
}
