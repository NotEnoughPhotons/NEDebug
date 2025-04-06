using UnityEngine;

namespace NEP.NEDebug
{
    public static class NELog
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

    public static class NEDraw
    {
        internal static NEDebugDrawer m_drawer;
        internal static bool m_ztest;

        public static void ZTest(bool enabled)
        {
            m_ztest = enabled;
        }

        public static void DrawLine(Vector3 p1, Vector3 p2, Color color)
        {
            if (!m_drawer)
            {
                return;
            }

            NEDrawCommand command = new NEDrawCommand();
            command.transform = Matrix4x4.identity;
            command.drawType = GL.LINE_STRIP;
            command.positions = [p1, p2];
            command.ztest = m_ztest;

            command.color = color;
            m_drawer.PushCommand(command);
        }

        public static void DrawRay(Ray ray, Color color)
        {
            DrawLine(ray.origin, ray.origin + ray.direction, color);
        }

        public static void DrawPlane(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
        {
            if (!m_drawer)
            {
                return;
            }

            NEDrawCommand command = new NEDrawCommand();
            command.transform = Matrix4x4.identity;
            command.transform *= Matrix4x4.TRS(position, rotation, scale);
            command.drawType = GL.LINE_STRIP;
            command.ztest = m_ztest;

            command.positions = [
                Vector3.up * -0.5f + new Vector3(0, 0, 0),
                Vector3.up * -0.5f + new Vector3(1, 0, 0),
                Vector3.up * -0.5f + new Vector3(1, 0, 0),
                Vector3.up * -0.5f + new Vector3(1, 1, 0),
                Vector3.up * -0.5f + new Vector3(1, 1, 0),
                Vector3.up * -0.5f + new Vector3(0, 1, 0),
                Vector3.up * -0.5f + new Vector3(0, 1, 0),
                Vector3.up * -0.5f + new Vector3(0, 0, 0)];

            command.color = color;
            m_drawer.PushCommand(command);
        }

        public static void DrawBox(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
        {
            if (!m_drawer)
            {
                return;
            }

            NEDrawCommand command = new NEDrawCommand();
            command.transform = Matrix4x4.identity;
            command.transform *= Matrix4x4.TRS(position, rotation, scale);
            command.drawType = GL.LINE_STRIP;
            command.ztest = m_ztest;

            command.positions = [
                Vector3.one * -0.5f + new Vector3(0, 0, 0),
                Vector3.one * -0.5f + new Vector3(1, 0, 0),
                Vector3.one * -0.5f + new Vector3(1, 0, 0),
                Vector3.one * -0.5f + new Vector3(1, 1, 0),
                Vector3.one * -0.5f + new Vector3(1, 1, 0),
                Vector3.one * -0.5f + new Vector3(0, 1, 0),
                Vector3.one * -0.5f + new Vector3(0, 1, 0),
                Vector3.one * -0.5f + new Vector3(0, 0, 0),
                Vector3.one * -0.5f + new Vector3(0, 0, 1),
                Vector3.one * -0.5f + new Vector3(1, 0, 1),
                Vector3.one * -0.5f + new Vector3(1, 0, 1),
                Vector3.one * -0.5f + new Vector3(1, 1, 1),
                Vector3.one * -0.5f + new Vector3(1, 1, 1),
                Vector3.one * -0.5f + new Vector3(0, 1, 1),
                Vector3.one * -0.5f + new Vector3(0, 1, 1),
                Vector3.one * -0.5f + new Vector3(0, 0, 1),
                Vector3.one * -0.5f + new Vector3(0, 0, 0),
                Vector3.one * -0.5f + new Vector3(0, 0, 1),
                Vector3.one * -0.5f + new Vector3(0, 0, 1),
                Vector3.one * -0.5f + new Vector3(0, 1, 1),
                Vector3.one * -0.5f + new Vector3(0, 1, 1),
                Vector3.one * -0.5f + new Vector3(0, 1, 0),
                Vector3.one * -0.5f + new Vector3(0, 1, 0),
                Vector3.one * -0.5f + new Vector3(0, 0, 0),
                Vector3.one * -0.5f + new Vector3(1, 0, 0),
                Vector3.one * -0.5f + new Vector3(1, 0, 1),
                Vector3.one * -0.5f + new Vector3(1, 0, 1),
                Vector3.one * -0.5f + new Vector3(1, 1, 1),
                Vector3.one * -0.5f + new Vector3(1, 1, 1),
                Vector3.one * -0.5f + new Vector3(1, 1, 0),
                Vector3.one * -0.5f + new Vector3(1, 1, 0),
                Vector3.one * -0.5f + new Vector3(1, 0, 0)];

            command.color = color;
            m_drawer.PushCommand(command);
        }

        public static void DrawDisc(Vector3 position, Quaternion rotation, Color color, float radius = 1.0f)
        {
            if (!m_drawer)
            {
                return;
            }

            NEDrawCommand command = new NEDrawCommand();
            command.positions = new List<Vector3>();
            command.transform = Matrix4x4.identity;
            command.transform *= Matrix4x4.TRS(position, rotation, Vector3.one);
            command.drawType = GL.LINE_STRIP;
            command.ztest = m_ztest;

            int sides = 32;

            for (int i = 0; i < sides + 1; i++)
            {
                float a = i / (float)sides;
                float angleA = a * Mathf.PI * 2f;

                float b = (i + 1) / (float)sides;
                float angleB = b * Mathf.PI * 2f;

                Vector3 initial = new Vector3(Mathf.Cos(angleA), 0f, Mathf.Sin(angleA));
                Vector3 next = new Vector3(Mathf.Cos(angleB), 0f, Mathf.Sin(angleB));

                command.positions.Add(position + initial * radius);
                command.positions.Add(position + next * radius);
            }

            command.color = color;
            m_drawer.PushCommand(command);
        }

        public static void DrawCylinder(Vector3 position, Quaternion rotation, Color color, float height = 1.0f, float radius = 1.0f)
        {
            if (!m_drawer)
            {
                return;
            }

            NEDrawCommand command = new NEDrawCommand();
            command.positions = new List<Vector3>();
            command.transform = Matrix4x4.identity;
            command.transform *= Matrix4x4.TRS(position, rotation, Vector3.one);
            command.drawType = GL.LINES;
            command.ztest = m_ztest;

            int sides = 32;

            for (int i = 0; i < sides + 1; i++)
            {
                float a = i / (float)sides;
                float angleA = a * Mathf.PI * 2f;

                float b = (i + 1) / (float)sides;
                float angleB = b * Mathf.PI * 2f;

                Vector3 initial = new Vector3(radius * Mathf.Cos(angleA), 0f, radius * Mathf.Sin(angleA));
                Vector3 next = new Vector3(radius * Mathf.Cos(angleB), 0f, radius * Mathf.Sin(angleB));

                command.positions.Add(initial);
                command.positions.Add(next);
            }
            
            for (int i = 0; i < 4; i++)
            {
                float a = i / (float)4;
                float angle = a * Mathf.PI * 2f;

                Vector3 p1 = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
                Vector3 p2 = new Vector3(radius * Mathf.Cos(angle), height, radius * Mathf.Sin(angle));

                command.positions.Add(p1);
                command.positions.Add(p2);
            }
            
            for (int i = 0; i < sides + 1; i++)
            {
                float a = i / (float)sides;
                float angleA = a * Mathf.PI * 2f;

                float b = (i + 1) / (float)sides;
                float angleB = b * Mathf.PI * 2f;

                Vector3 initial = new Vector3(radius * Mathf.Cos(angleA), height, radius * Mathf.Sin(angleA));
                Vector3 next = new Vector3(radius * Mathf.Cos(angleB), height, radius * Mathf.Sin(angleB));

                command.positions.Add(initial);
                command.positions.Add(next);
            }

            command.color = color;
            m_drawer.PushCommand(command);
        }

        public static void DrawSphere(Vector3 position, Quaternion rotation, Color color, float radius = 1.0f)
        {
            if (!m_drawer)
            {
                return;
            }

            NEDrawCommand command = new NEDrawCommand();
            command.positions = new List<Vector3>();
            command.transform = Matrix4x4.identity;
            command.transform *= Matrix4x4.TRS(position, rotation, Vector3.one);
            command.drawType = GL.LINES;
            command.ztest = m_ztest;

            int sides = 32;

            // X axis
            for (int i = 0; i < sides + 1; i++)
            {
                float a = i / (float)sides;
                float angleA = a * Mathf.PI * 2f;

                float b = (i + 1) / (float)sides;
                float angleB = b * Mathf.PI * 2f;

                Vector3 initial = new Vector3(Mathf.Cos(angleA), 0f, Mathf.Sin(angleA));
                Vector3 next = new Vector3(Mathf.Cos(angleB), 0f, Mathf.Sin(angleB));

                command.positions.Add(initial * radius);
                command.positions.Add(next * radius);
            }

            // Y axis
            for (int i = 0; i < sides + 1; i++)
            {
                float a = i / (float)sides;
                float angleA = a * Mathf.PI * 2f;

                float b = (i + 1) / (float)sides;
                float angleB = b * Mathf.PI * 2f;

                Vector3 initial = new Vector3(Mathf.Cos(angleA), Mathf.Sin(angleA), 0f);
                Vector3 next = new Vector3(Mathf.Cos(angleB), Mathf.Sin(angleB), 0f);

                command.positions.Add(initial * radius);
                command.positions.Add(next * radius);
            }
            
            // Z axis
            for (int i = 0; i < sides + 1; i++)
            {
                float a = i / (float)sides;
                float angleA = a * Mathf.PI * 2f;

                float b = (i + 1) / (float)sides;
                float angleB = b * Mathf.PI * 2f;

                Vector3 initial = new Vector3(0f, Mathf.Cos(angleA), Mathf.Sin(angleA));
                Vector3 next = new Vector3(0f, Mathf.Cos(angleB), Mathf.Sin(angleB));

                command.positions.Add(initial * radius);
                command.positions.Add(next * radius);
            }
            
            command.color = color;
            m_drawer.PushCommand(command);
        }

        internal static void Initialize()
        {
            if (!m_drawer)
            {
                GameObject instanceObject = new GameObject("NEDebug");
                instanceObject.hideFlags = HideFlags.DontUnloadUnusedAsset;
                m_drawer = instanceObject.AddComponent<NEDebugDrawer>();
            }
        }

        internal static void UnInitialize()
        {

        }
    }
}
