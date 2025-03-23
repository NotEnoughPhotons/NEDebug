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
            m_instance.DrawLine(p1, p2, color);
        }

        public static void DrawPlane(Vector3 position)
        {
            m_instance.DrawLine(new Vector3(0, 0, 0), new Vector3(1, 0, 0), Color.white);
            m_instance.DrawLine(new Vector3(1, 0, 0), new Vector3(1, 1, 0), Color.white);
            m_instance.DrawLine(new Vector3(1, 1, 0), new Vector3(0, 1, 0), Color.white);
            m_instance.DrawLine(new Vector3(0, 1, 0), new Vector3(0, 0, 0), Color.white);
        }

        public static void DrawBox(Vector3 position)
        {
            m_instance.DrawLine(new Vector3(0, 0, 0), new Vector3(1, 0, 0), Color.white);
            m_instance.DrawLine(new Vector3(1, 0, 0), new Vector3(1, 1, 0), Color.white);
            m_instance.DrawLine(new Vector3(1, 1, 0), new Vector3(0, 1, 0), Color.white);
            m_instance.DrawLine(new Vector3(0, 1, 0), new Vector3(0, 0, 0), Color.white);

            m_instance.DrawLine(new Vector3(0, 0, 1), new Vector3(1, 0, 1), Color.white);
            m_instance.DrawLine(new Vector3(1, 0, 1), new Vector3(1, 1, 1), Color.white);
            m_instance.DrawLine(new Vector3(1, 1, 1), new Vector3(0, 1, 1), Color.white);
            m_instance.DrawLine(new Vector3(0, 1, 1), new Vector3(0, 0, 1), Color.white);

            m_instance.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 1), Color.white);
            m_instance.DrawLine(new Vector3(0, 0, 1), new Vector3(0, 1, 1), Color.white);
            m_instance.DrawLine(new Vector3(0, 1, 1), new Vector3(0, 1, 0), Color.white);
            m_instance.DrawLine(new Vector3(0, 1, 0), new Vector3(0, 0, 0), Color.white);

            m_instance.DrawLine(new Vector3(1, 0, 0), new Vector3(1, 0, 1), Color.white);
            m_instance.DrawLine(new Vector3(1, 0, 1), new Vector3(1, 1, 1), Color.white);
            m_instance.DrawLine(new Vector3(1, 1, 1), new Vector3(1, 1, 0), Color.white);
            m_instance.DrawLine(new Vector3(1, 1, 0), new Vector3(1, 0, 0), Color.white);
        }

        public static void DrawDisc()
        {
            int sides = 32;

            for (int i = 0; i < sides; i++)
            {
                float a = i / (float)sides;
                float angle = a * Mathf.PI * 2;

                m_instance.DrawLine(new Vector3(0f, 0f, 0f), new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)), Color.white);
                m_instance.DrawLine(new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)), new Vector3(0f, 0f, 0f), Color.white);
            }
        }

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
