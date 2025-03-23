using UnityEngine;
using UnityEngine.Rendering;

namespace NEP.NEDebug
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    internal sealed class NEDebugVisualizer : MonoBehaviour
    {
        public NEDebugVisualizer(System.IntPtr ptr) : base(ptr) { }

        private Stack<NEDrawCommand> m_command;
        private Action<ScriptableRenderContext, Il2CppSystem.Collections.Generic.List<Camera>> m_callback;

        private void Awake()
        {
            m_command = new Stack<NEDrawCommand>();

            m_callback = new Action<ScriptableRenderContext, Il2CppSystem.Collections.Generic.List<Camera>>((ctx, cameras) => OnEndContextRendering(ctx, cameras));
            RenderPipelineManager.endContextRendering += m_callback;
        }

        private void OnDestroy()
        {
            m_command.Clear();
            m_command = null;
        }

        private void OnEndContextRendering(ScriptableRenderContext context, Il2CppSystem.Collections.Generic.List<Camera> cameras)
        {
            GL.PushMatrix();

            GL.MultMatrix(transform.localToWorldMatrix);

            GL.Begin(GL.LINE_STRIP);
            
            while (m_command.Count > 0)
            {
                NEDrawCommand command = m_command.Pop();
                for (int i = 0; i < command.positions.Count; i++)
                {
                    GL.Vertex(command.positions[i]);
                }
            }

            GL.End();
            GL.PopMatrix();

            m_command.Clear();
        }

        internal void DrawLine(Vector3 p1, Vector3 p2, Color color)
        {
            NEDrawCommand command = new NEDrawCommand();
            command.positions = [p1, p2];
            m_command.Push(command);
        }
    }
}
