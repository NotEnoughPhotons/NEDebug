using BoneLib;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

namespace NEP.NEDebug
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    internal sealed class NEDebugVisualizer : MonoBehaviour
    {
        public NEDebugVisualizer(System.IntPtr ptr) : base(ptr) { }

        private Action<ScriptableRenderContext, Il2CppSystem.Collections.Generic.List<Camera>> m_urpRenderCallback;

        private Stack<NEDrawCommand> m_commands;
        private Material m_material;
        private Camera m_vrCamera;

        private void Awake()
        {
            if (!Core.m_visMaterial)
            {
                NEDebug.Logger.Error("Failed to get visual material shader!");
            }
            else
            {
                m_material = new Material(Core.m_visMaterial);
            }

            m_commands = new Stack<NEDrawCommand>();
            m_urpRenderCallback += OnEndContextRendering;
            m_vrCamera = Player.ControllerRig.cameras[0];
            NEDebug.Logger.Log($"Got {m_vrCamera.name} as the main NEDebug drawing camera");
        }

        private void OnDestroy()
        {
            m_urpRenderCallback -= OnEndContextRendering;
            m_commands.Clear();
            m_commands = null;
        }

        private void OnEndContextRendering(ScriptableRenderContext context, Il2CppSystem.Collections.Generic.List<Camera> cameras)
        {
            DebugDraw();
        }

        private void OnRenderObject()
        {
            DebugDraw();
        }

        private void DebugDraw()
        {
            if (!m_material)
            {
                return;
            }

            while (m_commands.Count > 0)
            {
                NEDrawCommand command = m_commands.Pop();

                GL.PushMatrix();
                m_material.SetPass(0);

                GL.MultMatrix(command.transform);

                GL.Begin(command.drawType);

                GL.Color(command.color);

                for (int i = 0; i < command.positions.Count; i++)
                {
                    GL.Vertex(command.positions[i]);
                }

                GL.End();

                GL.PopMatrix();
            }

            m_commands.Clear();
        }

        internal void DrawLine(Vector3 p1, Vector3 p2, Color color)
        {
            DrawLine(p1, p2, Quaternion.identity, Vector3.one, color);
        }

        internal void DrawLine(Vector3 p1, Vector3 p2, Quaternion rotation, Color color)
        {
            DrawLine(p1, p2, rotation, Vector3.one, color);
        }

        internal void DrawLine(Vector3 p1, Vector3 p2, Vector3 scale, Color color)
        {
            DrawLine(p1, p2, Quaternion.identity, scale, color);
        }

        internal void DrawLine(Vector3 p1, Vector3 p2, Quaternion rotation, Vector3 scale, Color color)
        {
            NEDrawCommand command = new NEDrawCommand();
            command.transform = Matrix4x4.identity;
            command.drawType = GL.LINE_STRIP;
            command.positions = [p1, p2];

            command.color = color;
            m_commands.Push(command);
        }

        internal void DrawPlane(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
        {
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
            m_commands.Push(command);
        }

        internal void DrawBox(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
        {
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
            m_commands.Push(command);
        }
    }
}
