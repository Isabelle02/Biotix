using UnityEngine;

public abstract class Popup : MonoBehaviour
{
    protected abstract void OnOpenStart(ViewParam viewParam);

    protected abstract void OnCloseStart();

    public void Open(ViewParam viewParam)
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

