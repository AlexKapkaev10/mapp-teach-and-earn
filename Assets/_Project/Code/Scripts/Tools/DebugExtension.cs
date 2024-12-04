using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Project.Infrastructure.Extensions
{
    public static class DebugExtension
    {
        private const string UNITY_EDITOR = "UNITY_EDITOR";
        private const string DEV_BUILD = "DEVELOPMENT_BUILD";

        public static string Color(this string myStr, string color)
        {
            return $"<color={color}>{myStr}</color>";
        }

        private static void TryUpdateLogsMap(string logName, out bool isLog)
        {
            isLog = true;

#if UNITY_EDITOR
            if (Logs.TryAdd(logName, true))
            {
                LogsMapUpdated.Invoke();
                Logs.TryGetValue(logName, out isLog);
            }
#endif
        }

#if UNITY_EDITOR
        public static Dictionary<string, bool> Logs = new();
        public static event Action LogsMapUpdated = delegate { };

        [InitializeOnLoadMethod]
        private static void InEditorCallback()
        {
            EditorApplication.playModeStateChanged += OnModeChanged;
        }

        private static async void OnModeChanged(PlayModeStateChange mode)
        {
            await ClearLogsAsync(mode);
        }

        private static async Task ClearLogsAsync(PlayModeStateChange mode)
        {
            await Task.Delay(TimeSpan.FromSeconds(1.0f));

            if (mode == PlayModeStateChange.ExitingPlayMode)
            {
                Logs.Clear();
                LogsMapUpdated.Invoke();
            }
        }
#endif

        #region MonoBehaviour

        private static void DoLog(Action<string, Object> LogFunction, string prefix, Object myObj, params object[] msg)
        {
#if UNITY_EDITOR || DEBUG
            var objName = myObj ? myObj.name : "NullObject";
            var name = objName.Color("lightblue");
            TryUpdateLogsMap(objName, out var isLog);

            if (isLog)
            {
                LogFunction($"{prefix}[{name}]: {String.Join("; ", msg)}\n ", myObj);
            }
#endif
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void Log(this Object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "", myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogError(this Object myObj, params object[] msg)
        {
            DoLog(Debug.LogError, "<!>".Color("red"), myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogWarning(this Object myObj, params object[] msg)
        {
            DoLog(Debug.LogWarning, "⚠️".Color("yellow"), myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogSuccess(this Object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "☻".Color("green"), myObj, msg);
        }

        #endregion

        #region Native

        private static void DoLog(Action<string> LogFunction, string prefix, object myObj, params object[] msg)
        {
#if UNITY_EDITOR || DEBUG

            var objName = myObj != null ? myObj.GetType().ToString() : "NullObject";
            var logName = objName.Color("lightblue");
            TryUpdateLogsMap(objName, out var isLog);

            if (isLog)
            {
                LogFunction($"{prefix}[{logName}]: {String.Join("; ", msg)}\n ");
            }
#endif
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void Log(this object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "", myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogError(this object myObj, params object[] msg)
        {
            DoLog(Debug.LogError, "<!>".Color("red"), myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogWarning(this object myObj, params object[] msg)
        {
            DoLog(Debug.LogWarning, "⚠️".Color("yellow"), myObj, msg);
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogSuccess(this object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "☻".Color("green"), myObj, msg);
        }
        
        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogDispose(this object myObj)
        {
            DoLog(Debug.Log, "○".Color("green"), myObj, "has been disposed!");
        }
        
        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogInitializeSuccess(this object myObj)
        {
            DoLog(Debug.Log, "☺".Color("green"), myObj, "has been initialized!");
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogRegisterSuccess(this object myObj)
        {
            DoLog(Debug.Log, "☐".Color("green"), myObj, "has been registered!");
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogInjectSuccess(this object myObj)
        {
            DoLog(Debug.Log, "┣▇▇▇═─".Color("green"), myObj, "has been injected!");
        }

        [Conditional(UNITY_EDITOR)]
        [Conditional(DEV_BUILD)]
        public static void LogSubscribeSuccess(this object myObj)
        {
            DoLog(Debug.Log, "☊".Color("green"), myObj, "has been subscribed!");
        }

        #endregion
    }
}