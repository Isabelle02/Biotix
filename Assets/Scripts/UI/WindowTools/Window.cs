using UnityEngine;

public abstract class Window : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
    
    public abstract void OnOpenStart(Window.ViewParam viewParam);
    public abstract void OnCloseStart();

    public void Open(Window.ViewParam viewParam)
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