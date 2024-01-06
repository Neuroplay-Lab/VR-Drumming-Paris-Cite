using System;
using System.Collections.Generic;
using _Project.Scripts.Systems;
using DrumRhythmGame.Systems;
using UnityEngine;
using TMPro;

namespace _Project.Scripts.UI
{
    /// <summary>
    ///     A simple console logger that re-directs the latest log message.
    /// </summary>
    public class ConsoleLogger : MonoBehaviour
    {
        [SerializeField] private GameObject consoleLog;

        private static List<LogEntry> logs = new List<LogEntry>();

        private TextMeshProUGUI logText;
        private bool isRunning;

        /// <summary>
        ///     Checks if console logging is enabled.
        /// </summary>
        private void Awake()
        {
            isRunning = SaveData.Instance.preferenceData.enableLogging;

            if (transform.GetChild(0).TryGetComponent(out logText))
                logText.text = ($"Logging is: {(isRunning ? "ON" : "OFF")}");
            else
                enabled = false;
        }

        private void OnEnable()
        {
            // A very useful built-in
            Application.logMessageReceived += HandleLogMessage;
            // Check if we switched off logging during runtime
            EventManager.LoggingStateChanged += ctx =>
            {
                isRunning = ctx;
                if(!ctx) logText.text = "Logging is: <b>OFF</b>";
            };

        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLogMessage;
            EventManager.LoggingStateChanged += ctx =>
            {
                isRunning = ctx;
                if(!ctx) logText.text = "Logging is: <b>OFF</b>";
            };

        }

        private void HandleLogMessage(string message, string stack, LogType type)
        {
            LogEntry log = new LogEntry(message, stack, type);
            logs.Add(log);
            
            if(isRunning) DisplayOnCanvasConsole(log);
        }

        /// <summary>
        ///     Displays the latest log message on the canvas.
        /// </summary>
        /// <param name="logEntry">The log entry to display</param>
        private void DisplayOnCanvasConsole(LogEntry logEntry)
        {
            if(!logText) return;
            logText.text = logEntry.message;
        }
    }
    
    [Serializable]
    public class LogEntry
    {
        public string message;
        public string stackTrace;
        public LogType type;
        
        public LogEntry() { }

        public LogEntry(string message, string stackTrace, LogType type)
        {
            this.message = message;
            this.stackTrace = stackTrace;
            this.type = type;
        }
    }
}
