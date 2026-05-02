using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;
    static readonly object _lock = new();
    static bool _applicationIsQuitting;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance of {typeof(T)} was requested after the application quit. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();

                    if (_instance == null)
                    {
                        var singletonObject = new GameObject($"{typeof(T).Name} (Singleton)");
                        _instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);

                        Debug.Log($"[Singleton] Created instance of {typeof(T)}.");
                    }
                }
            }
            
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T)} found on {gameObject.name}. Destroying it.");
            Destroy(gameObject);
        }
    }


    protected virtual void OnApplicationQuit()
        => _applicationIsQuitting = true;

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
