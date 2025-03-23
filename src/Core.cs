using System.Reflection;

using MelonLoader;

using UnityEngine;

using Il2CppSLZ.Marrow;

using BoneLib;

[assembly: MelonInfo(typeof(NEP.NEDebug.Core), "NEDebug", "1.0.0", "Not Enough Photons: adamdev", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace NEP.NEDebug
{
    public class Core : MelonMod
    {
        internal static MelonLogger.Instance m_logger;
        internal static Material m_visMaterial;

        public Transform playerTorso;

        public override void OnInitializeMelon()
        {
            m_logger = Melon<Core>.Logger;
            string packDir = HelperMethods.IsAndroid() ? "nedraw_shaders_quest.pack" : "nedraw_shaders.pack";
            AssetBundle bundle = HelperMethods.LoadEmbeddedAssetBundle(Assembly.GetExecutingAssembly(), "NEP.NEDebug.Resources." + packDir);
            m_visMaterial = bundle.LoadPersistentAsset<Material>("VisDraw");
            Hooking.OnLevelLoaded += OnLevelLoaded;
        }

        public void OnLevelLoaded(LevelInfo info)
        {
            NEDebug.Initialize();
        }

        public override void OnUpdate()
        {
#if DEBUG
            if (!Player.HandsExist)
            {
                return;
            }

            PhysicsRig rig = Player.PhysicsRig;
            Vector3 pos = rig.torso.transform.position;
            Quaternion rot = rig.torso.transform.rotation;
            NEDebug.DrawBox(pos, rot, Color.blue);
            NEDebug.DrawLine(pos, Vector3.zero, Color.red);

            Transform lhand = rig.leftHand.transform;

            NEDebug.DrawLine(lhand.position, lhand.position + lhand.right * 0.1f, Color.red);
            NEDebug.DrawLine(lhand.position, lhand.position + lhand.forward * 0.1f, Color.blue);
            NEDebug.DrawLine(lhand.position, lhand.position + lhand.up * 0.1f, Color.green);

            Ray ray = new Ray(pos, rig.torso.transform.forward * 0.5f);
            NEDebug.DrawRay(ray, Color.yellow);
            // NEDebug.DrawBox(Vector3.zero, Quaternion.identity, Color.red);
#endif
        }
    }
}