using UnityEngine;

public static class MouseManager
{
    public static Vector3 GetMousePosition(float order)
    {
        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, order);
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
    
    public static T GetObject<T>()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity,  LayerMask.GetMask("UI"));
        if (hit.collider != null)
            if (hit.collider.gameObject.TryGetComponent(out T node))
                return node;

        return default;
    }
}