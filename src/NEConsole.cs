using System.Reflection;

using UnityEngine;
using UnityEngine.EventSystems;

using Il2CppTMPro;

using MelonLoader;
using HarmonyLib;

namespace NEP.NEDebug.Console
{
    public static class NEConsole
    {
        internal static class Compatibility
        {
            private static HarmonyLib.Harmony m_harmony;
            
            private static Assembly m_flatPlayerAssembly;
            private static MethodInfo m_flatPlayerUpdate;

            internal static void SetHarmonyInstance(HarmonyLib.Harmony instance)
            {
                m_harmony = instance;
            }
            
            internal static void FlatPlayerSetup()
            {
                IEnumerable<MelonAssembly> assemblies = MelonAssembly.LoadedAssemblies;
                
                foreach (MelonAssembly assembly in assemblies)
                {
                    if (assembly.Assembly.GetName().Name == "FlatPlayer")
                    {
                        m_flatPlayerAssembly = assembly.Assembly;
                        break;
                    }
                }

                if (m_flatPlayerAssembly == null)
                {
                    Core.m_logger.Warning("Couldn't find the FlatPlayer mod!");
                    return;
                }

                Type[] types = m_flatPlayerAssembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.FullName == "FlatPlayer.FlatBooter")
                    {
                        object singleton = type.GetField("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null);

                        if (singleton == null)
                        {
                            Core.m_logger.Error("Couldn't find singleton of FlatPlayer!");
                            continue;
                        }

                        m_flatPlayerUpdate = singleton.GetType().GetMethod("OnLateUpdate", BindingFlags.Public | BindingFlags.Instance);
                        Core.m_logger.Msg("Found the FlatPlayer update method!");
                        break;
                    }
                }
                
                PatchFlatPlayer();
            }

            internal static void PatchFlatPlayer()
            {
                if (m_flatPlayerAssembly == null || m_flatPlayerUpdate == null)
                {
                    return;
                }

                MethodInfo originalMethodInfo = m_flatPlayerUpdate;
                MethodInfo targetMethodInfo = typeof(Compatibility).GetMethod("FlatPlayer_OnLateUpdate_Prefix", BindingFlags.NonPublic | BindingFlags.Static);

                m_harmony.Patch(originalMethodInfo, prefix: new HarmonyMethod(targetMethodInfo));
            }

            private static bool FlatPlayer_OnLateUpdate_Prefix()
            {
                if (m_consoleGo == null)
                {
                    return true;
                }

                if (m_showConsole)
                {
                    return false;
                }

                return true;
            }
        }
        
        public static Dictionary<string, MethodInfo> Commands = new();

        private static GameObject m_consoleGo;
        private static TMP_InputField m_consoleInput;
        private static EventSystem m_eventSystem;

        private static bool m_showConsole;

        internal static void Initialize()
        {
            m_consoleGo = GameObject.Instantiate(Core.m_consoleObject);
            m_consoleGo.SetActive(false);
            m_consoleInput = m_consoleGo.transform.Find("Console/InputField").GetComponent<TMP_InputField>();
            m_eventSystem = m_consoleGo.GetComponent<EventSystem>();
            m_consoleInput.onSubmit.AddListener(new Action<string>((input) => { Execute(input); }));
            ScanAssemblies();
        }
        
        internal static void ScanAssemblies()
        {
            IEnumerable<MelonMod> mods = MelonMod.RegisteredMelons;
                
            foreach (var mod in mods)
            {
                Assembly assembly = mod.MelonAssembly.Assembly;
                
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods())
                    {
                        NEConsoleCommand command = method.GetCustomAttribute<NEConsoleCommand>();
                                    
                        if (command == null)
                        {
                            continue;
                        }
                                    
                        AddCommand(command.Command, method);
                    }
                }
            }
        }
        
        public static void AddCommand(string command, MethodInfo method)
        {
            Commands.Add(command, method);
        }

        public static void Execute(string command)
        {
            if (Commands.TryGetValue(command, out MethodInfo method))
            {
                if (method.IsStatic)
                {
                    method.Invoke(null, null);
                }
            }
        }

        public static void ShowConsole()
        {
            m_showConsole = !m_showConsole;
            Cursor.visible = m_showConsole;
            m_consoleGo.SetActive(m_showConsole);
            EventSystem.current = m_eventSystem;
        }

        public static void FocusConsole()
        {
            m_consoleInput.Select();
        }
    }
}