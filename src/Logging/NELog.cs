namespace NEP.NEDebug
{
    public static class NELog
    {
        public static void Log(string message)
        {
            Core.m_logger.Msg(message);
        }

        public static void Warning(string message)
        {
            Core.m_logger.Warning(message);
        }

        public static void Error(string message)
        {
            Core.m_logger.Error(message);
        }
    }
}