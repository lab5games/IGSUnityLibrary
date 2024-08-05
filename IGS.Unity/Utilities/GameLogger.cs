using System;
using System.Text;

namespace IGS.Unity
{
    public enum LogFilter : byte
    {
        Log = 0,
        Warning = 1,
        Error = 2,

        System = 10,
        Network = 11,
        
        Trace = 20
    }

    public static class GameLogger
    {
        internal class ConsoleLogger
        {
            const int FONT_SIZE = 14;
            const string LOG_COLOR = "#FFFFFFFF";
            const string WARNING_COLOR = "#FFFF00FF";
            const string ERROR_COLOR = "#FF0000FF";
            const string TRACE_COLOR = "#FF8000FF";
            const string SYSTEM_COLOR = "#00FF00FF";
            const string NETWORK_COLOR = "#00FFFFFF";

            public static void Log(string inLog, string logFilter, UnityEngine.Object context)
            {
                UnityEngine.Debug.Log(BuildLog(inLog, logFilter), context);
            }

            private static string BuildLog(string inLog, string logFilter)
            {
                string colorStr = GetColorString(logFilter);
                
                StringBuilder strBuilder = new StringBuilder();
                BuildFilter(strBuilder, logFilter, colorStr);
                BuildTime(strBuilder, colorStr);
                BuildLog(strBuilder, inLog, colorStr);

                return strBuilder.ToString();
            }

            private static void BuildFilter(StringBuilder strBuilder, string logFilter, string color)
            {
                string s = string.Format("[{0}]", logFilter.Trim().ToUpper());
                s = s.PadRight(s.Length + 2);
                strBuilder.Append(s.Size(FONT_SIZE).Color(color));
            }

            private static void BuildTime(StringBuilder strBuilder, string color)
            {
                string s = string.Format("[{0} ({1})]", DateTime.Now.ToString("HH:mm:ss"), UnityEngine.Time.frameCount);
                s = s.PadRight(s.Length + 2);
                strBuilder.Append(s.Size(FONT_SIZE).Color(color));
            }

            private static void BuildLog(StringBuilder strBuilder, string inLog, string color)
            {
                string s = string.Format("{0}", inLog);
                strBuilder.Append(s.Size(FONT_SIZE).Bold().Color(color));
            }

            private static string GetColorString(string logFilter)
            {
                if(string.IsNullOrEmpty(logFilter))
                    return LOG_COLOR;

                switch(logFilter.Trim().ToUpper())
                {
                    case "WARNING": 
                        return WARNING_COLOR;
                    case "ERROR":
                        return ERROR_COLOR;
                    case "SYSTEM":
                        return SYSTEM_COLOR;
                    case "NETWORK":
                        return NETWORK_COLOR;
                    case "TRACE":
                        return TRACE_COLOR;
                    default:
                        return LOG_COLOR;
                }
            }
        }

        public delegate void LogDelegate(string inLog, string logFilter, UnityEngine.Object context);
        public static LogDelegate LogCallback;

        static GameLogger()
        {
            LogCallback = ConsoleLogger.Log;
        }

        public static void Log(string inLog, string logFilter, UnityEngine.Object context)
        {
            LogCallback(inLog, logFilter, context);
        }

        public static void Log(string inLog, LogFilter logFilter, UnityEngine.Object context = null)
        {
            Log(inLog, logFilter.ToString(), context);
        }

        public static void Log(object obj, string logFilter, UnityEngine.Object context = null)
        {
            Log(obj.ToString(), logFilter, context);
        }

        public static void Log(object obj, LogFilter logFilter, UnityEngine.Object context = null)
        {
            Log(obj.ToString(), logFilter.ToString(), context);
        }

        public static string Bold(this string str)
        {
            return string.Format("<b>{0}</b>", str);
        }

        public static string Italic(this string str)
        {
            return string.Format("<i>{0}</i>", str);
        }

        public static string Color(this string str, string color)
        {
            return string.Format("<color={0}>{1}</color>", color, str);
        }

        public static string Size(this string str, int size)
        {
            return string.Format("<size={0}>{1}</size>", size, str);
        }
    }
}
