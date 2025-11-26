using Il2CppSLZ.Marrow;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace NEP.NEDebug
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    internal sealed class NEDebugDrawer : MonoBehaviour
    {
        public NEDebugDrawer(System.IntPtr ptr) : base(ptr) { }

        private Action<ScriptableRenderContext, Il2CppSystem.Collections.Generic.List<Camera>> m_urpRenderCallback;

        private Stack<NEDrawCommand> m_commands;
        private Material m_material;
        private Camera m_vrCamera;
        private TextMeshPro m_textMesh;

        private void Awake()
        {
            if (!Core.m_visMaterial)
            {
                NELog.Error("Failed to get visual material shader!");
            }
            else
            {
                m_material = new Material(Core.m_visMaterial);
            }

            m_commands = new Stack<NEDrawCommand>();
            m_urpRenderCallback += OnEndContextRendering;
            OpenControllerRig controllerRig = FindObjectOfType<OpenControllerRig>();
            m_vrCamera = controllerRig.cameras[0];
            NELog.Log($"Got {m_vrCamera.name} as the main NEDebug drawing camera");
            
            m_textMesh = GameObject.Instantiate(Core.m_textObject).GetComponent<TextMeshPro>();
            m_textMesh.transform.localScale = new Vector3(-1, 1, 1);
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

                if (m_textMesh != null)
                {
                    if (command.text != string.Empty)
                    {
                        m_textMesh.text = command.text;
                        m_textMesh.fontSize = command.textSize;
                        m_textMesh.transform.position = new Vector3(command.transform[0, 3], command.transform[1, 3], command.transform[2, 3]);
                        m_textMesh.transform.rotation = Quaternion.LookRotation(m_vrCamera.transform.position - m_textMesh.transform.position, Vector3.up);
                    }
                }

                if (command.drawType == 0)
                {
                    continue;
                }
                
                GL.PushMatrix();
                m_material.SetPass(0);
                m_material.SetFloat("_ZTest", command.ztest ? 0.0f : 4.0f);

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

        internal void PushCommand(NEDrawCommand command)
        {
            m_commands.Push(command);
        }
    }
}
