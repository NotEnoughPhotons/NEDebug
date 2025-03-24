using UnityEngine;

namespace NEP.NEDebug
{
    public static class NEDebug
    {
        public static class Logger
        {
            public static void Log(string message)
            {
                Core.m_logger.Msg(message);
            }

            public static void Warning(string message)
            {
                Core.m_logger.Warning(message);
            }

            public static void Error(string message)
            {
                Core.m_logger.Error(message);
            }
        }

        internal static NEDebugVisualizer m_instance;

        public static void DrawLine(Vector3 p1, Vector3 p2, Color color)
        {
            if (m_instance == null)
            {
                return;
            }

            m_instance.DrawLine(p1, p2, color);
        }

        public static void DrawRay(Ray ray, Color color)
        {
            if (m_instance == null)
            {
                return;
            }

            m_instance.DrawLine(ray.origin, ray.origin + ray.direction, color);
        }

        public static void DrawPlane(Vector3 position, Color color)
        {
            if (m_instance == null)
            {
                return;
            }

            m_instance.DrawPlane(position, Quaternion.identity, Vector3.one, color);
        }

        public static void DrawPlane(Vector3 position, Vector3 scale, Color color)
        {
            if (m_instance == null)
            {
                return;
            }

            m_instance.DrawPlane(position, Quaternion.identity, scale, color);
        }

        public static void DrawPlane(Vector3 position, Quaternion rotation, Color color)
        {
            if (m_instance == null)
            {
                return;
            }

            m_instance.DrawPlane(position, rotation, Vector3.one, color);
        }

        public static void DrawPlane(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
        {
            if (m_instance == null)
            {
                return;
            }

            m_instance.DrawPlane(position, rotation, scale, color);
        }

        public static void DrawBox(Vector3 position, Color color)
        {
            if (m_instance == null)
            {
                return;
            }

            m_instance.DrawBox(position, Quaternion.identity, Vector3.one, color);
        }

        public static void DrawBox(Vector3 position, Quaternion rotation, Color color)
        {
            if (m_instance == null)
            {
                return;
            }

            m_instance.DrawBox(position, rotation, Vector3.one, color);
        }

        public static void DrawBox(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
        {
            if (m_instance == null)
            {
                return;
            }

            m_instance.DrawBox(position, rotation, scale, color);
        }

#if DEBUG
        // TODO: Reimplement so I can reuse this for cylinders and spheres
        public static void DrawDisc(Vector3 position)
        {
            if (m_instance == null)
            {
                return;
            }

            int sides = 32;

            for (int i = 0; i < sides + 1; i++)
            {
                float a = i / (float)sides;
                float angle = a * Mathf.PI * 2;

                m_instance.DrawLine(position + new Vector3(0f, 0f, 0f), position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)), Color.white);
                m_instance.DrawLine(position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)), position + new Vector3(0f, 0f, 0f), Color.white);
            }
        }
#endif

        internal static void Initialize()
        {
            if (m_instance == null)
            {
                GameObject instanceObject =  new GameObject("NEDebug");
                instanceObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
                m_instance = instanceObject.AddComponent<NEDebugVisualizer>();
            }
        }

        internal static void UnInitialize()
        {
            
        }
    }
}
