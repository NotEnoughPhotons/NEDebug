using System.Reflection;

namespace NEP.NEDebug.Console
{
    public static class NEConsole
    {
        public struct ConsoleMethodGroup
        {
            public ConsoleMethodGroup(string method, string command)
            {
                this.method = method;
                this.command = command;
            }
            
            public string method;
            public string command;
        }

        public static IReadOnlyList<ConsoleMethodGroup> Methods => m_methods.AsReadOnly();

        private static List<ConsoleMethodGroup> m_methods;

        public static void Initialize()
        {
            m_methods = new List<ConsoleMethodGroup>();
        }

        public static void AddCommand(string method, string command)
        {
            ConsoleMethodGroup group = new ConsoleMethodGroup(method, command);
            m_methods.Add(group);
        }
    }
}