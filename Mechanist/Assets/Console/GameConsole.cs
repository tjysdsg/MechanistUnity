using System;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

namespace Console
{
    public class GameConsole : MonoBehaviour
    {
        [SerializeField] private int maxLength = 1000;
        [SerializeField] private Text text;

        private FileSystemWatcher _watcher;
        private FileStream _fileStream;
        private StreamReader _reader;
        private string _content = "GameConsole\n";
        private bool _changed = false;

        private void OnEnable()
        {
            var (logDir, logFile) = GetLogFilePath();
            string path = Path.Combine(logDir, logFile);
            if (!File.Exists(path))
                return;

            _watcher = new FileSystemWatcher(logDir, logFile);
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.InternalBufferSize = 4096 * 50;
            _watcher.Changed += OnChanged;
            // _watcher.Error += (object sender, ErrorEventArgs e) => { Debug.Log("BIG SHIT"); };
            _watcher.EnableRaisingEvents = true;

            _fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite);
            _reader = new StreamReader(_fileStream);
        }

        private void OnDisable()
        {
            if (_watcher != null)
            {
                _watcher.Changed -= OnChanged;
                _watcher.Dispose();
            }

            if (_fileStream != null)
            {
                _fileStream.Close();
                _reader.Close();
            }
        }

        /// <summary>
        /// https://docs.unity3d.com/Manual/LogFiles.html
        /// </summary>
        private (string, string) GetLogFilePath()
        {
            string logDir = "";
            string logFile = "";
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    logDir =
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "Unity", "Editor"
                        );
                    logFile = "Editor.log";
                    break;
                case RuntimePlatform.WindowsPlayer:
                    logDir =
                        Path.GetFullPath(
                            Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..",
                                "LocalLow",
                                Application.companyName, Application.productName
                            ));
                    logFile = "Player.log";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return (logDir, logFile);
        }

        private void Update()
        {
            if (_changed)
            {
                _content += _reader.ReadToEnd();
                if (_content.Length > maxLength)
                    _content = _content.Substring(_content.Length - maxLength, maxLength);

                text.text = _content;
                _changed = false;
                _watcher.EnableRaisingEvents = true;
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            _changed = true;
            _watcher.EnableRaisingEvents = false;
        }
    }
}