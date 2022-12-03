using UnityEngine;

public static class ObjectExtension
{
    public static void DestroyObjectsOfType<T>(this object obj) where T : MonoBehaviour
    {
        var objects = Object.FindObjectsOfType<T>();
        for (var i = objects.Length - 1; i >= 0; i--)
            Object.DestroyImmediate(objects[i].gameObject);
    }
}