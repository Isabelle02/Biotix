using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UnitView : MonoBehaviour, IReleasable
{
    [SerializeField] private Image _teamImage;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private int _minSize;
    [SerializeField] private int _maxSize;
    
    private Tween _runningTween;
    private Tween _moveAnimTween;

    public UnitEntity UnitEntity;

    public NodeEntity TargetNode;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<NodeView>(out var nodeView))
        {
            if (TargetNode == nodeView.NodeEntity)
            {
                WorldManager.CurrentWorld.GetSystem<NodeSystem>().GetHit(TargetNode, UnitEntity);
                WorldManager.CurrentWorld.RemoveEntity(UnitEntity);
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
        _moveAnimTween = PlayMoveAnim(duration / 10);
    }

    private bool _isRunning;

    private Tween PlayMoveAnim(float duration)
    {
        return DOTween.To(() => _minSize,
            x => _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x),
            _maxSize,
            duration).SetLoops(-1, LoopType.Yoyo).OnStepComplete(() =>
        {
            _isRunning = !_isRunning;
            if (_isRunning)
                _runningTween?.Play();
            else
                _runningTween?.Pause();
        });
    }

    public void SetPause(bool isPaused)
    {
        if (isPaused)
        {
            _runningTween.Pause();
            _moveAnimTween.Pause();
        }
        else
        {
            _runningTween.Play();
            _moveAnimTween.Play();
        }
    }
    
    public void Dispose()
    {
        _runningTween?.Kill();
        _moveAnimTween?.Kill();
    }
}
