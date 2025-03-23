using System.Runtime.InteropServices;

using UnityEngine;

namespace NEP.NEDebug
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NEDrawCommand
    {
        public int drawType;
        public List<Vector3> positions;
        public Matrix4x4 transform;
        public Color color;
    }
}
