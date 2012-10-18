using System.Collections.Generic;

namespace LedCubeClient
{
    public static class ClientLogger
    {
        private static readonly object LockObj = new object();
        public static List<string> Log = new List<string>();

        public static List<string> GetLog()
        {
            lock(LockObj)
            {
                var l = new List<string>();
                foreach(var i in Log)
                {
                    l.Add((string)i.Clone());
                    Log.Remove(i);
                }
                return l;
            }
        }
        
        public static void WriteLine(string message)
        {
            Logger.Logger.WriteLine(message);
        }

        public static void WriteLine(string format, params object[] args)
        {
            Logger.Logger.WriteLine(string.Format(format, args));
        }
    }
}
