using System.Reflection;
using UnityEngine.Playables;

namespace NEP.NEDebug.Console
{
    public static class NECommandTokenizer
    {
        public static string[] SplitCommandInput(string input)
        {
            return input.Split('\t', ' ');
        }

        public static List<NECommandToken> Tokenize(NEConsoleCommand command, string[] rawTokens)
        {
            List<NECommandToken> tokens = new List<NECommandToken>();

            ParameterInfo[] parameters = command.Method.GetParameters();
            int paramIndex = 0;
            foreach (var rawToken in rawTokens)
            {
                if (rawToken == command.Command)
                {
                    continue;
                }
                
                NECommandToken token = new NECommandToken();
                token.command = command;
                token.token = rawToken;

                if (parameters.Length > 0)
                {
                    ParameterInfo parameter = parameters[paramIndex++];
                    token.type = parameter.ParameterType;
                }
                
                tokens.Add(token);
            }

            return tokens;
        }
    }
}