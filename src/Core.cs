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
        internal static MelonLogger.Instance m_logger;
        internal static Material m_visMaterial;

        internal static GameObject m_consoleObject;
        
        public override void OnInitializeMelon()
        {
            m_logger = Melon<Core>.Logger;
            string packDir = HelperMethods.IsAndroid() ? "nedraw_shaders_quest.pack" : "nedraw_shaders.pack";
            AssetBundle bundle = HelperMethods.LoadEmbeddedAssetBundle(Assembly.GetExecutingAssembly(), "NEP.NEDebug.Resources." + packDir);
            m_visMaterial = bundle.LoadPersistentAsset<Material>("VisDraw");
            m_consoleObject = bundle.LoadPersistentAsset<GameObject>("ConsoleBar");
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

        [NEConsoleCommand("testfunction")]
        public static void TestFunction()
        {
            Melon<Core>.Logger.Msg("Hello world!");
        }
    }
}