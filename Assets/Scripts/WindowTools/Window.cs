using DG.Tweening;
using UnityEngine;

public abstract class Window : MonoBehaviour
{
    [SerializeField] private GameObject _showPanel;

    private void Start() => Initialize();

    protected abstract void Initialize();

    protected abstract void OnOpenStart(ViewParam viewParam);

    protected abstract void OnCloseStart();

    public void Open(ViewParam viewParam = null)
    {
        var y = _showPanel.transform.localScale.y;
        var z = _showPanel.transform.localScale.z;
        _showPanel.transform.localScale.Set(0, y, z);
        gameObject.SetActive(true);
        _showPanel.transform.DOScaleX(1, 0.25f).OnComplete(() => OnOpenStart(viewParam));
    }

    public void Close()
    {
        _showPanel.transform.DOScaleX(0, 0.25f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            OnCloseStart();
        });
    }
}

public abstract class ViewParam
{
}