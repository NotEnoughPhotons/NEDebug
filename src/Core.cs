using System.Reflection;

using MelonLoader;

using UnityEngine;

using BoneLib;

using NEP.NEDebug.Console;
using UnityEngine.EventSystems;

[assembly: MelonInfo(typeof(NEP.NEDebug.Core), "NEDebug", "0.0.4", "Not Enough Photons: adamdev", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace NEP.NEDebug
{
    internal class Core : MelonMod
    {
        internal static AssetBundle m_assetBundle;
        internal static MelonLogger.Instance m_logger;
        internal static Material m_visMaterial;

        internal static GameObject m_consoleObject;

        private void LoadBundles()
        {
            string packDir = HelperMethods.IsAndroid() ? "nedebug_quest.pack" : "nedebug.pack";
            m_assetBundle = HelperMethods.LoadEmbeddedAssetBundle(Assembly.GetExecutingAssembly(), "NEP.NEDebug.Resources." + packDir);

            if (m_assetBundle == null)
            {
                m_logger.BigError("Failed to load the NEDebug bundle!");
                return;
            }
            
            m_visMaterial = m_assetBundle.LoadPersistentAsset<Material>("VisDraw");
            m_consoleObject = m_assetBundle.LoadPersistentAsset<GameObject>("ConsoleBar");
            
            NEConsole.ScanAssemblies();
        }
        
        public override void OnInitializeMelon()
        {
            m_logger = Melon<Core>.Logger;
            LoadBundles();
            NEConsole.Compatibility.SetHarmonyInstance(HarmonyInstance);
            NEConsole.Compatibility.FlatPlayerSetup();
            Hooking.OnLevelLoaded += (info) =>
            {
                NEDraw.Initialize();
                NEConsole.Initialize();
            };
        }

        public override void OnDeinitializeMelon()
        {
            NEDraw.UnInitialize();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                NEConsole.ShowConsole();
                NEConsole.FocusConsole();
            }
        }
    }
}