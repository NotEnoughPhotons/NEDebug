using System.Reflection;
using System.Runtime.CompilerServices;

namespace NEP.NEDebug.Console
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NEConsoleCommand : Attribute
    {
        public NEConsoleCommand(string command)
        {
            m_command = command;
        }

        public string Command
        {
            get
            {
                if (m_command != string.Empty)
                {
                    return m_command;
                }
                else
                {
                    return "null";
                }
            }
        }
        
        private string m_command;
    }
}