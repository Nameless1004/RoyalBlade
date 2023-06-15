using System.Collections.Generic;
using UnityEngine;

public static class ResourceCache
{
    public static readonly Dictionary<string, Object> _resources = new Dictionary<string, Object>();
    
    public static T GetResource<T>(string path) where T : Object
    {
        if(_resources.TryGetValue(path, out var obj) == false)
        {
            obj = Resources.Load<T>(path);
            _resources.Add(path, obj);
        }
        return obj as T;
    }

}
