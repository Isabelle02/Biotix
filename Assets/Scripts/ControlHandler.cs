using System;
using UnityEngine;

public class ControlHandler : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            HandleClick();
    }

    private void HandleClick()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue);
        if (!hit) 
            return;
        
        var component = hit.collider.GetComponent(typeof(Component));
        if (!component)
            return;

        if (component.TryGetComponent<IClickable>(out var clickable)) 
            clickable.CLick();
    }
}

public interface IControl
{
    
}

public interface IClickable : IControl
{
    public void CLick();
}