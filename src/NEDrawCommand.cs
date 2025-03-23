using System.Runtime.InteropServices;

using UnityEngine;

namespace NEP.NEDebug
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NEDrawCommand
    {
        public Matrix4x4 matrix;
        public List<Vector3> positions;
    }
}
