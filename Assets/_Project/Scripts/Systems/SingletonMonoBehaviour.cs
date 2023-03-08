using UnityEngine;

namespace _Project.Scripts.Systems
{
    /// <summary>
    /// A Singleton pattern that can be used for MonoBehaviour.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var t = typeof(T);
                _instance = (T) FindObjectOfType(t);
                if (_instance == null) 
                    Debug.LogError(t + " does not have a GameObject attached to it.");

                return _instance;
            }
        }

        public static bool Instantiated => _instance != null;

        #region Event Functions

        protected virtual void Awake()
        {
            // Check if it is attached to another GameObject.
            if (this != Instance)
            {
                Destroy(this);
                //Destroy(this.gameObject);
                Debug.LogWarning(
                    typeof(T) +
                    " has already been attached to another GameObject, so the component was destroyed - " + Instance.gameObject.name);
                return;
            }

            // If you want to enable this GameObject across Manager-like Scenes
            // ↓ Please remove the comment.
            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}