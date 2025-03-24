using System.Reflection;

using MelonLoader;

using UnityEngine;

using Il2CppSLZ.Marrow;

using BoneLib;
using System.Diagnostics;

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
#endif
        }
    }
}