using UnityEngine;

public abstract class Page : MonoBehaviour
{
    protected abstract void OnOpenStart(ViewParam viewParam);

    protected abstract void OnCloseStart();

    public void Open(ViewParam viewParam = null)
    {
        gameObject.SetActive(true);
        OnOpenStart(viewParam);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        OnCloseStart();
    }
    
    public abstract class ViewParam
    {
    }
}