using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    /// <summary>
    ///     A class responsible for logging data to a CSV file.
    /// </summary>
    [HelpURL("https://learn.microsoft.com/en-us/dotnet/api/system.idisposable?view=net-7.0")]
    public class DataLogger : IDisposable
    {
        private readonly string _logDirectory;
        private IDisposable _coroutine;

        private bool _isRecording;
        private Queue<string> _queue;
        //private Queue<(byte[], float)> _screenshots;

        private SaveData _saveData;

        public DataLogger()
        {
            string baseLogDir = $@"{Application.dataPath}/Log";
            if (!Directory.Exists(baseLogDir))
                // Create directory when it doesn't exist
                Directory.CreateDirectory(baseLogDir);

            // get details of the current participant, based on the previous
            ParticipantData currentPpt = ParticipantData.GetNextParticipantID();
            _logDirectory = baseLogDir + $"/{currentPpt.date}-ppt{currentPpt.pptNumber}/Sync Logs";
            currentPpt.Save(); // update most recent participant to this one in save data

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);

            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            StopRecording();
        }

        #endregion

        public void StartRecording(string csvLabel = "Label")
        {
            _queue = new Queue<string>();
            _coroutine = Observable.FromCoroutine(() => StartRecordingTransform(csvLabel)).Subscribe();
            _saveData = SaveData.Instance;
        }

        public void StopRecording()
        {
            if (_isRecording)
            {
                _isRecording = false;
                _coroutine.Dispose();
            }
        }

        public void Enqueue(string text)
        {
            _queue?.Enqueue(text);
        }

        private IEnumerator StartRecordingTransform(string csvLabel)
        {
            // TODO: Could probably use a better file name, and allow the user to specify it
            var filePath = _logDirectory + $@"/{DateTime.Now:yyyy-MM-dd-HHmmss}.csv";
            _isRecording = true;

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Write label
                writer.WriteLine(csvLabel);

                while (_isRecording)
                {
                    if (_queue.Count != 0)
                    {
                        var text = _queue.Dequeue();
                        if (_saveData.preferenceData.enableLogging) Debug.Log(text);

                        writer.WriteLine(text);
                        writer.Flush();
                    }

                    /*if (_screenshots.Count > 0)
                    {
                        var (ss, t) = _screenshots.Dequeue();

                        System.IO.File.WriteAllBytes(screenshotFolderPath + $@"/{t}.png", ss);

                    }*/

                    yield return null;
                }
            }

            _queue.Clear();
        }
    }
}