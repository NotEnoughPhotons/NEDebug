using System.Reflection;
using System.Runtime.CompilerServices;

namespace NEP.NEDebug.Console
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NEConsoleCommand : Attribute
    {
        public NEConsoleCommand(string command)
        {
            Core.m_logger.Msg("console command added");
            
            m_command = command;
            
            NEConsole.AddCommand(null, m_command);
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

        public string Caller
        {
            get
            {
                return m_caller;
            }
        }
        
        private string m_command;
        private string m_caller;
    }
}