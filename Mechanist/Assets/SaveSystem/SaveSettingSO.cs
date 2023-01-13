using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SaveSystem
{
    public class SaveSettingSO : ScriptableObject
    {
        private const string FILENAME = "SaveSystemSettings";

        private static SaveSettingSO instance;

        private void OnDestroy()
        {
            instance = null;
        }

        public static SaveSettingSO Get()
        {
            if (instance != null)
                return instance;

            var savePluginSettings = Resources.Load(FILENAME, typeof(SaveSettingSO)) as SaveSettingSO;

#if UNITY_EDITOR
            // In case the settings are not found, we create one
            if (savePluginSettings == null)
                return CreateFile();
#endif

            // In case it still doesn't exist, somehow it got removed.
            // We send a default instance of SavePluginSettings.
            if (savePluginSettings == null)
            {
                Debug.LogWarning(
                    "Could not find SavePluginsSettings in resource folder, did you remove it? Using default settings.");
                savePluginSettings = ScriptableObject.CreateInstance<SaveSettingSO>();
            }

            instance = savePluginSettings;
            return instance;
        }

#if UNITY_EDITOR
        public static SaveSettingSO CreateFile()
        {
            string resourceFolderPath = $"{Application.dataPath}/Resources";
            string filePath = $"{resourceFolderPath}/{FILENAME}.asset";

            // In case the directory doesn't exist, we create a new one.
            if (!Directory.Exists(resourceFolderPath))
                UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");

            // Check if the settings file exists in the resources path
            // If not, we create a new one.
            if (!File.Exists(filePath))
            {
                instance = ScriptableObject.CreateInstance<SaveSettingSO>();
                UnityEditor.AssetDatabase.CreateAsset(instance, $"Assets/Resources/{FILENAME}.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
                return instance;
            }
            else
            {
                return Resources.Load(FILENAME, typeof(SaveSettingSO)) as SaveSettingSO;
            }
        }

        private void OnValidate()
        {
            fileExtensionName = ValidateString(fileExtensionName, ".savegame", false);
            fileFolderName = ValidateString(fileFolderName, "SaveData", true);
            fileName = ValidateString(fileName, "Slot", true);

            if (fileExtensionName[0] != '.')
            {
                Debug.LogWarning("SaveSettings: File extension name needs to start with a .");
                fileExtensionName = $".{fileExtensionName}";
            }
        }

        private string ValidateString(string input, string defaultString, bool allowWhiteSpace)
        {
            if (string.IsNullOrEmpty(input) || (!allowWhiteSpace && input.Any(Char.IsWhiteSpace)))
            {
                Debug.LogWarning($"SaveSettings: Set {input} back to {defaultString} " +
                                 "since it was empty or has whitespace.");
                return defaultString;
            }
            else
            {
                return input;
            }
        }
#endif

        [Header("Storage Settings")] public string fileExtensionName = ".savegame";
        public string fileFolderName = "SaveData";
        public string fileName = "Slot";
        public bool useJsonPrettyPrint = true;

        [Header("Configuration")] [Range(1, 300)]
        public int maxSaveSlotCount = 300;

        [Header("Auto Save")]
        [Tooltip("Automatically save to the active slot based on a time interval, useful for WEBGL games")]
        public bool saveOnInterval = false;

        [Tooltip("Time interval in seconds before the autosave happens"), Range(1, 3000)]
        public int saveIntervalTime = 1;

        [Header("Debug (Unity Editor Only)")] public bool showSaveFileUtilityLog = false;
    }
}