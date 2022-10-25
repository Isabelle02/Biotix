using UnityEngine;

public abstract class Page : MonoBehaviour, IPage
{
    public bool IsActive => gameObject.activeSelf;
    
    public abstract void OnOpenStart(IPage.ViewParam viewParam);
    public abstract void OnCloseStart();

    public void Open(IPage.ViewParam viewParam)
    {
        gameObject.SetActive(true);
        OnOpenStart(viewParam);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
        OnCloseStart();
    }
}