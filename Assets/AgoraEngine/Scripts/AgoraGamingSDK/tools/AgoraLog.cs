using UnityEngine;

namespace agora_gaming_rtc
{
    public sealed class AgoraLog
    {
        internal static string tag = "Agora_Unity: ";

        public static void Log_Debug(string message)
        {
            Debug.Log(tag + message);
        }

        public static void Log_Error(string message)
        {
            Debug.LogError(tag + message);
        }
    }
}