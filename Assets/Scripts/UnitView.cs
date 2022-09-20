using System;
using UnityEngine;

public class UnitView : MonoBehaviour, IReleasable
{
    public event Action Disposed;
    
    public void Dispose()
    {
        Disposed?.Invoke();
    }
}
