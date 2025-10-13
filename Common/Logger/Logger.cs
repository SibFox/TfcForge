using System;
using System.Text;
using Godot;

// internal delegate string GetFlatStr(string str);

namespace TfcForge.Common.Logger
{
    public class Logger()
    {
        private string header = null;
        private StringBuilder logString = new();
        private bool toDebug = false;
        private NotificationType notificationType = NotificationType.Info;

        public enum NotificationType
        {
            Info,
            Warning,
            Error
        }

        public static Logger LogInfo(params string[] headers)
        {
            Logger log = new()
            {
                notificationType = NotificationType.Info
            };
            SetHeader(ref log, headers);
            return log;
        }

        public static Logger LogWarn(params string[] headers)
        {
            Logger log = new()
            {
                notificationType = NotificationType.Warning
            };
            SetHeader(ref log, headers);
            return log;
        }

        public static Logger LogErr(params string[] headers)
        {
            Logger log = new()
            {
                notificationType = NotificationType.Error
            };
            SetHeader(ref log, headers);
            return log;
        }

        public static Logger DebugInfo(params string[] headers)
        {
            Logger log = new()
            {
                notificationType = NotificationType.Info,
                toDebug = true
            };
            SetHeader(ref log, headers);
            return log;
        }

        public static Logger DebugWarn(params string[] headers)
        {
            Logger log = new()
            {
                notificationType = NotificationType.Warning,
                toDebug = true
            };
            SetHeader(ref log, headers);
            return log;
        }

        public static Logger DebugErr(params string[] headers)
        {
            Logger log = new()
            {
                notificationType = NotificationType.Error,
                toDebug = true
            };
            SetHeader(ref log, headers);
            return log;
        }

        private static void SetHeader(ref Logger logger, params string[] headers)
        {
            if (headers.IsEmpty())
            {
                DebugErr(nameof(Logger), "SetHeader").AddLine("The headers array is empty").Push();
                return;
            }

            foreach (string head in headers)
                logger.header += head + "/";
                
            logger.header = logger.header[..^1].Replace(" ", "");
        }

        public Logger AddLine(params string[] strings)
        {
            if (strings.IsEmpty())
            {
                DebugErr(nameof(Logger), "AddLine", "String").AddLine("Parameters are empty").Push();
                return this;
            }

            string wholeLine = "";
            foreach (string str in strings)
                wholeLine += str + " ";

            logString.AppendLine(wholeLine);
            return this;
        }

        public Logger AddLine(params object[] objs)
        {
            if (objs == null)
            {
                DebugErr(nameof(Logger), "AddLine", "Object").AddLine("Parameters are empty").Push();
                return this;
            }

            StringBuilder stringBuilder = new();
            foreach (object obj in objs)
            {
                stringBuilder.Append(obj?.ToString() + " " ?? " //null// ");
            }

            logString.AppendLine(stringBuilder.ToString());
            return this;
        }

        /// <summary>
        /// Finishes log entry and pushes it to the terminal and saves to file
        /// </summary>
        public void Push()
        {
            if (logString.Length == 0)
            {
                DebugErr(nameof(Logger), "Push").AddLine("The log line is empty").Push();
                return;
            }

            StringBuilder finalString = new($"[{DateTime.Now:hh:mm:ss:fff}] ");
            if (toDebug)
                finalString.Append("[D] ");
            if (header != null)
                finalString.Append($"[{header}] ");

            switch (notificationType)
            {
                case NotificationType.Info:
                    GD.Print(finalString + logString.ToString().TrimEnd().Replace("\n", ""));
                    LogToFile(finalString);
                    break;
                case NotificationType.Warning:
                    GD.Print(finalString + "[!] " + logString.ToString().TrimEnd());
                    GD.PushWarning(finalString + logString.ToString().TrimEnd());
                    LogToFile(finalString);
                    break;
                case NotificationType.Error:
                    GD.PrintErr(finalString + logString.ToString().TrimEnd());
                    GD.PushError(finalString + logString.ToString().TrimEnd());
                    LogToFile(finalString);
                    break;
            }
        }

        private void LogToFile(StringBuilder finalString)
        {
            FileAccess debug = FileAccess.Open(Global.Paths.LogsPath + "debug.log", FileAccess.ModeFlags.ReadWrite);
            debug.SeekEnd();
            FileAccess latest = null;
            if (!toDebug)
            {
                latest = FileAccess.Open(Global.Paths.LogsPath + "latest.log", FileAccess.ModeFlags.ReadWrite);
                latest.SeekEnd();
            }
            finalString.Replace("[D] ", "", 1, 20);

            switch (notificationType)
            {
                case NotificationType.Info:
                    debug.StoreLine(finalString + "[INFO] " + logString.ToString().TrimEnd());
                    latest?.StoreLine(finalString + "[INFO] " + logString.ToString().TrimEnd());
                    break;
                case NotificationType.Warning:
                    debug.StoreLine(finalString + "[WARN] " + logString.ToString().TrimEnd());
                    latest?.StoreLine(finalString + "[WARN] " + logString.ToString().TrimEnd());
                    break;
                case NotificationType.Error:
                    debug.StoreLine(finalString + "[ERROR] " + logString.ToString().TrimEnd());
                    latest?.StoreLine(finalString + "[ERROR] " + logString.ToString().TrimEnd());
                    break;
            }
            debug.Close();
            latest?.Close();
        }
    }
}