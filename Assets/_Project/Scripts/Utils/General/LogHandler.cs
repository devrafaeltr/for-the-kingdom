using UnityEngine;
using System.Runtime.CompilerServices;
using System.IO;

namespace FTKingdom.Utils
{
    public static class LogHandler
    {
        public static void StateLog(string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "")
        {
            Log($"[State]\n{GetFormattedMessage(sourceFilePath, memberName)}{message}");
        }

        public static void SceneLog(string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "")
        {
            Log($"[Scene]\n{GetFormattedMessage(sourceFilePath, memberName)}{message}");
        }

        private static void Log(string message)
        {
            Debug.Log(message);
        }

        private static string GetFormattedMessage(string sourceFilePath, string memberName)
        {
            string className = Path.GetFileNameWithoutExtension(sourceFilePath);
            return $"[{className}][{memberName}] ";
        }
    }
}