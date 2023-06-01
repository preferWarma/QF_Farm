using System;

namespace Lyf.Utils.Singleton
{
    using UnityEngine;

    public class MaintainFirstSingleton<T> : MonoBehaviour, ISingleton, IDisposable where T : MaintainFirstSingleton<T>
    {
        private static readonly object Lock = new object();
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = FindObjectOfType<T>();
                            if (_instance == null)
                            {
                                Debug.LogWarning($"Cannot find {typeof(T)} in scene, creating a new one.");
                                GameObject obj = new GameObject(typeof(T).Name);
                                _instance = obj.AddComponent<T>();
                            }
                            DontDestroyOnLoad(_instance.gameObject);
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
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        public virtual void Dispose()
        {
            if (_instance == this)
            {
                _instance = null;
                Destroy(gameObject);
            }
        }
    }

}