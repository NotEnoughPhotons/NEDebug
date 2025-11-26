using System.Runtime.InteropServices;

using UnityEngine;

namespace NEP.NEDebug
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NEDrawCommand
    {
        public NEDrawCommand()
        {
            drawType = 0;
            positions = new List<Vector3>();
            transform =Matrix4x4.identity;
            color = Color.white;
            orthographic = false;
            ztest = false;
            text = string.Empty;
            textSize = 1f;
        }
        
        public int drawType;
        public List<Vector3> positions;
        public Matrix4x4 transform;
        public Color color;
        public bool orthographic;
        public bool ztest;
        public string text;
        public float textSize;
    }
}
