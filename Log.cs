using VRage.Utils;

namespace Catopia.ProtectionBlock
{
    public static class Log
    {
        public static bool DebugLog;
        public static void Msg(string msg)
        {
            MyLog.Default.WriteLine($"ProtectionBlock: {msg}");
        }

        public static void Debug(string msg)
        {
            if (DebugLog)
                Msg($"[DEBUG] {msg}");
        }
    }
}
