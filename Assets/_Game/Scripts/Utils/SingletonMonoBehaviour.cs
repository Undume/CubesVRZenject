using UnityEngine;

namespace SharedUtils
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected abstract bool DoNotDestroyOnLoad { get; }
        private static T m_instance = null;
        private static readonly System.Type m_instanceType = typeof(T);
        protected static bool m_isShuttingDown = false;

        public static T Instance
        {
            get
            {
                if (m_isShuttingDown)
                {
                    return null;
                }

                if (m_instance == null)
                {
                    m_instance = GameObject.FindObjectOfType(m_instanceType) as T;
                    if (m_instance == null)
                    {
                        var go = new GameObject(m_instanceType.ToString());
                        m_instance = go.AddComponent<T>();
                    }
                }

                return m_instance;
            }
        }

        private void Awake()
        {
            if (m_isShuttingDown)
            {
                m_instance = null;
                m_isShuttingDown = false;
            }

            if (m_instance != null && m_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (DoNotDestroyOnLoad) DontDestroyOnLoad(gameObject);
            SingletonAwake();
        }

        protected virtual void SingletonAwake()
        {
        }

        private void OnApplicationQuit()
        {
            m_isShuttingDown = true;
        }
    }
}