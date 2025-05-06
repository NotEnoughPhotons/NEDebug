using System.Reflection;

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

        public MethodInfo Method
        {
            get
            {
                return m_method;
            }
        }
        
        private string m_command;
        private MethodInfo m_method;

        public void SetMethod(MethodInfo method)
        {
            m_method = method;
        }
    }
}