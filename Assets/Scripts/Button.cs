using System;
using UnityEngine;

public class Button : MonoBehaviour, IClickable
{
    public event Action Clicked;
    public event Action ClickedOnce;
    
    public void CLick()
    {
        ClickedOnce?.Invoke();
        ClickedOnce = null;
        
        Clicked?.Invoke();
    }
}