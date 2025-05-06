using System.Reflection;

namespace NEP.NEDebug.Console
{
    public static class NEConsole
    {
        public static Dictionary<string, MethodInfo> Commands = new Dictionary<string, MethodInfo>();

        internal static void SweepAssembly()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods())
                {
                    NEConsoleCommand command = method.GetCustomAttribute<NEConsoleCommand>();
                    
                    if (command == null)
                    {
                        continue;
                    }
                    
                    Commands.Add(command.Command, method);
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
    }
}