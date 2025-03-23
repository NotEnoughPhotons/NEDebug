using MelonLoader;
using BoneLib;
using UnityEngine;
using Il2CppSLZ.Marrow;

[assembly: MelonInfo(typeof(NEP.NEDebug.Core), "NEDebug", "1.0.0", "Not Enough Photons: adamdev", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace NEP.NEDebug
{
    public class Core : MelonMod
    {
        internal static MelonLogger.Instance m_logger;

        public Transform playerTorso;

        public override void OnInitializeMelon()
        {
            m_logger = Melon<Core>.Logger;
            Hooking.OnLevelLoaded += OnLevelLoaded;
        }

        public void OnLevelLoaded(LevelInfo info)
        {
            NEDebug.Initialize();
        }

        public override void OnUpdate()
        {
            if (!Player.HandsExist)
            {
                return;
            }

            PhysicsRig rig = Player.PhysicsRig;
            Vector3 pos = rig.torso.transform.position;
            Quaternion rot = rig.torso.transform.rotation;
            NEDebug.DrawBox(pos, rot, Color.blue);
            NEDebug.DrawLine(pos, Vector3.zero, Color.red);
            // NEDebug.DrawBox(Vector3.zero, Quaternion.identity, Color.red);
        }
    }
}