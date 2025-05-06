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
        
        public static Dictionary<string, NEConsoleCommand> RegisteredCommands = new();

        private static GameObject m_consoleGo;
        private static TMP_InputField m_consoleInput;
        private static EventSystem m_eventSystem;
        private static EventSystem m_gameEventSystem;

        private static bool m_showConsole;

        internal static void Initialize()
        {
            m_consoleGo = GameObject.Instantiate(Core.m_consoleObject);
            m_consoleGo.SetActive(false);
            m_consoleInput = m_consoleGo.transform.Find("Console/InputField").GetComponent<TMP_InputField>();
            m_eventSystem = m_consoleGo.GetComponent<EventSystem>();
            m_consoleInput.onSubmit.AddListener(new Action<string>((input) => { Execute(input); }));
            m_gameEventSystem = GetGameEventSystem();
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
                                    
                        AddCommand(command, method);
                    }
                }
            }
        }
        
        public static void AddCommand(NEConsoleCommand command, MethodInfo method)
        {
            command.SetMethod(method);
            RegisteredCommands.Add(command.Command, command);
        }

        public static void Execute(string command)
        {
            string[] rawTokens = NECommandTokenizer.SplitCommandInput(command);
            
            if (!RegisteredCommands.TryGetValue(rawTokens[0], out NEConsoleCommand consoleCommand))
            {
                return;
            }
            
            List<NECommandToken> tokens = NECommandTokenizer.Tokenize(consoleCommand, rawTokens);

            MethodInfo commandMethod = consoleCommand.Method;
            ParameterInfo[] parameters = commandMethod.GetParameters();
            
            if (parameters.Length > 0)
            {
                List<object> arguments = new List<object>();
                
                for (int i = 0; i < tokens.Count; i++)
                {
                    NECommandToken token = tokens[i];
                    ParameterInfo parameter = parameters[i];
                    
                    Type tokenType = token.type;
                    Type parameterType = parameter.ParameterType;

                    if (tokenType != parameterType)
                    {
                        break;
                    }

                    object parsedValue = null;

                    if (parameterType == typeof(int) && int.TryParse(token.token, out int intValue))
                    {
                        parsedValue = intValue;
                    }
                    else if (parameterType == typeof(float) && float.TryParse(token.token, out float floatValue))
                    {
                        parsedValue = floatValue;
                    }
                    else if (parameterType == typeof(string))
                    {
                        parsedValue = token.token;
                    }

                    arguments.Add(parsedValue);
                }
                
                commandMethod.Invoke(null, arguments.ToArray());;
            }
            else
            {
                commandMethod.Invoke(null, null);
            }
        }

        public static void ShowConsole()
        {
            m_showConsole = !m_showConsole;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = m_showConsole;
            m_consoleGo.SetActive(m_showConsole);
            EventSystem.current = m_showConsole ? m_eventSystem : m_gameEventSystem;
        }

        public static void FocusConsole()
        {
            m_consoleInput.Select();
        }

        internal static EventSystem GetGameEventSystem()
        {
            EventSystem eventSystem = null;
            foreach (var system in EventSystem.m_EventSystems)
            {
                if (system.name == "LegacyUIEventSystem")
                {
                    eventSystem = system;
                    break;
                }
            }

            return eventSystem;
        }
    }
}