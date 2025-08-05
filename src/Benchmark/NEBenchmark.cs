using System.Diagnostics;
using System.Reflection;

using HarmonyLib;

namespace NEP.NEDebug.Benchmark
{
    public static class NEBenchmark
    {
        private static Stopwatch m_stopwatch;
        private static HarmonyLib.Harmony m_patcher;
        private static bool m_init = false;
        private static Type m_self;
        private static string m_name;
        

        // Methods with no return type
        public static void Profile(Action method)                                                         => Patch(method.GetMethodInfo());
        public static void Profile<T>(Action<T> method)                                                   => Patch(method.GetMethodInfo());
        public static void Profile<T1, T2>(Action<T1, T2> method)                                         => Patch(method.GetMethodInfo());
        public static void Profile<T1, T2, T3>(Action<T1, T2, T3> method)                                 => Patch(method.GetMethodInfo());
        public static void Profile<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method)                         => Patch(method.GetMethodInfo());
        public static void Profile<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method)                 => Patch(method.GetMethodInfo());
        public static void Profile<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method)         => Patch(method.GetMethodInfo());
        public static void Profile<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method) => Patch(method.GetMethodInfo());
        
        // Methods with return types
        public static void Profile<T1, RTT>(Func<T1, RTT> method)                                         => Patch(method.GetMethodInfo());
        public static void Profile<T1, T2, RTT>(Func<T1, T2, RTT> method)                                 => Patch(method.GetMethodInfo());
        
        private static void Patch(MethodInfo method)
        {
            if (method == null)
            {
                NELog.Warning("NEBenchmark::Profile - Method is null!");
                return;
            }
            
            if (!m_init)
            {
                m_stopwatch = new Stopwatch();
                m_patcher = new HarmonyLib.Harmony("NEBenchmark_Harmony");
                m_init = true;
                m_self = Type.GetType("NEP.NEDebug.Benchmark.NEBenchmark");
                m_name = string.Empty; 
            }
            
            MethodInfo stopwatchPrefixMethod = m_self.GetMethod("Prefix", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo stopwatchPostfixMethod = m_self.GetMethod("Postfix", BindingFlags.NonPublic | BindingFlags.Static);

            HarmonyMethod stopwatchPrefix = new HarmonyMethod(stopwatchPrefixMethod);
            HarmonyMethod stopwatchPostfix = new HarmonyMethod(stopwatchPostfixMethod);

            m_name = method.Name;
            
            m_patcher.Patch(method, stopwatchPrefix, stopwatchPostfix);
        }
        
        private static bool Prefix()
        {
            m_stopwatch.Start();
            return true;
        }

        private static void Postfix()
        {
            if (m_stopwatch.ElapsedMilliseconds != 0)
            {
                NELog.Log($"{m_name} :: {m_stopwatch.ElapsedMilliseconds}ms");
            }
            
            m_stopwatch.Restart();
        }
    }
}