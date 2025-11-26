using UnityEngine;

namespace NEP.NEDebug.Pooling
{
    public static class NEPool<T> where T : MonoBehaviour
    {
        private static int m_capacity = 6;
        private static T m_instance;
        
        public static void Initialize(T instance)
        {
            if (instance == null)
            {
                throw new NullReferenceException("Missing pool instance!");
            }
            
            m_instance = instance;
        }

        public static void Spawn(Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
        {
            
        }
    }
}