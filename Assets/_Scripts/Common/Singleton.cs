using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singleton
{
    /// <summary>
    /// A Static instance is similar to a singleton, but instead of destroying any
    /// new instances, it overrides the current instance. This is handy for resetting the state
    /// and save you doing it manually
    /// </summary>
    public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        protected virtual void Awake() => Instance = this as T;

        protected virtual void OnApplicationQuit()
        {
            Instance = null;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This tranforms the static instance into a basic singleton. This will destroy any new
    /// version create, leaving the original instance intact
    /// </summary>
    public abstract class Singleton<T> : StaticInstance<T> where T: MonoBehaviour
    {
        protected override void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            base.Awake();
        }
    }

    /// <summary>
    /// This will survive through scene load. Perfect for system classes which require stateful,
    /// persistent data. Or audio sources where music play through loading screens, etc
    /// 
    /// </summary>
    public abstract class PersistenSingleton<T> : Singleton<T> where T: MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}

