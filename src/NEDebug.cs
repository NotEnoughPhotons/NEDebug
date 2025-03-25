using Il2CppSystem.Net;
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

        internal static NEDebugDrawer m_drawer;

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

            command.positions = [
                Vector3.up * -0.5f + new Vector3(0, 0, 0), Vector3.up * -0.5f + new Vector3(1, 0, 0),
                Vector3.up * -0.5f + new Vector3(1, 0, 0), Vector3.up * -0.5f + new Vector3(1, 1, 0),
                Vector3.up * -0.5f + new Vector3(1, 1, 0), Vector3.up * -0.5f + new Vector3(0, 1, 0),
                Vector3.up * -0.5f + new Vector3(0, 1, 0), Vector3.up * -0.5f + new Vector3(0, 0, 0)];

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

            command.positions = [
                Vector3.one * -0.5f + new Vector3(0, 0, 0), Vector3.one * -0.5f + new Vector3(1, 0, 0),
                Vector3.one * -0.5f + new Vector3(1, 0, 0), Vector3.one * -0.5f + new Vector3(1, 1, 0),
                Vector3.one * -0.5f + new Vector3(1, 1, 0), Vector3.one * -0.5f + new Vector3(0, 1, 0),
                Vector3.one * -0.5f + new Vector3(0, 1, 0), Vector3.one * -0.5f + new Vector3(0, 0, 0),
                Vector3.one * -0.5f + new Vector3(0, 0, 1), Vector3.one * -0.5f + new Vector3(1, 0, 1),
                Vector3.one * -0.5f + new Vector3(1, 0, 1), Vector3.one * -0.5f + new Vector3(1, 1, 1),
                Vector3.one * -0.5f + new Vector3(1, 1, 1), Vector3.one * -0.5f + new Vector3(0, 1, 1),
                Vector3.one * -0.5f + new Vector3(0, 1, 1), Vector3.one * -0.5f + new Vector3(0, 0, 1),
                Vector3.one * -0.5f + new Vector3(0, 0, 0), Vector3.one * -0.5f + new Vector3(0, 0, 1),
                Vector3.one * -0.5f + new Vector3(0, 0, 1), Vector3.one * -0.5f + new Vector3(0, 1, 1),
                Vector3.one * -0.5f + new Vector3(0, 1, 1), Vector3.one * -0.5f + new Vector3(0, 1, 0),
                Vector3.one * -0.5f + new Vector3(0, 1, 0), Vector3.one * -0.5f + new Vector3(0, 0, 0),
                Vector3.one * -0.5f + new Vector3(1, 0, 0), Vector3.one * -0.5f + new Vector3(1, 0, 1),
                Vector3.one * -0.5f + new Vector3(1, 0, 1), Vector3.one * -0.5f + new Vector3(1, 1, 1),
                Vector3.one * -0.5f + new Vector3(1, 1, 1), Vector3.one * -0.5f + new Vector3(1, 1, 0),
                Vector3.one * -0.5f + new Vector3(1, 1, 0), Vector3.one * -0.5f + new Vector3(1, 0, 0)];

            command.color = color;
            m_drawer.PushCommand(command);
        }

        public static void DrawDisc(Vector3 position, float radius = 1.0f)
        {
            DrawDisc(position, Quaternion.identity, radius);
        }

        public static void DrawDisc(Vector3 position, Quaternion rotation, float radius = 1.0f)
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

            command.color = Color.white;
            m_drawer.PushCommand(command);
        }

        public static void DrawCylinder(Vector3 position, float height = 1.0f, float radius = 1.0f)
        {
            if (!m_drawer)
            {
                return;
            }

            NEDrawCommand command = new NEDrawCommand();
            command.positions = new List<Vector3>();
            command.transform = Matrix4x4.identity;
            command.transform *= Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
            command.drawType = GL.LINES;

            DrawDisc(position, radius);
            DrawDisc(position + Vector3.up * height * 0.5f, radius);

            for (int i = 0; i < 4; i++)
            {
                float a = i / (float)4;
                float angle = a * Mathf.PI * 2f;

                Vector3 p1 = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
                Vector3 p2 = new Vector3(radius * Mathf.Cos(angle), height, radius * Mathf.Sin(angle));

                command.positions.Add(position + p1);
                command.positions.Add(position + p2);
            }

            command.color = Color.white;
            m_drawer.PushCommand(command);
        }

        public static void DrawSphere(Vector3 position, float radius = 1.0f)
        {
#if DEBUG
            if (!m_drawer)
            {
                return;
            }

            NEDrawCommand command = new NEDrawCommand();
            command.positions = new List<Vector3>();
            // command.transform = Matrix4x4.identity;
            // command.transform *= Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
            command.drawType = GL.LINE_STRIP;

            // TODO: This is a HACK!
            // For some reason it won't look at the world right axis.
            // The fix for now is to rotate it by 1 radian on the X axis, and then rotate -
            // the Y axis forcefully.
            // Could be because of a bad matrix multiplication.
            DrawDisc(position, Quaternion.LookRotation(Vector3.right + (Vector3.up * 1000f)), radius);
            DrawDisc(position, Quaternion.LookRotation(Vector3.up), radius);
            DrawDisc(position, Quaternion.LookRotation(Vector3.forward), radius);

            command.color = Color.white;
            m_drawer.PushCommand(command);
#endif
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
