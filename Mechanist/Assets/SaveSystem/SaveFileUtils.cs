using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace SaveSystem
{
    public class SaveFileUtils
    {
        private static string fileExtentionName => SaveSettingSO.Get().fileExtensionName;

        private static string gameFileName => SaveSettingSO.Get().fileName;

        private static bool debugMode => SaveSettingSO.Get().showSaveFileUtilityLog;

        private static string DataPath => $"{Application.persistentDataPath}/{SaveSettingSO.Get().fileFolderName}";

        private static void Log(string text)
        {
            if (debugMode)
                Debug.Log(text);
        }

        private static Dictionary<int, string> cachedSavePaths = null;

        public static Dictionary<int, string> ObtainSavePaths()
        {
            if (cachedSavePaths != null)
                return cachedSavePaths;

            Dictionary<int, string> newSavePaths = new Dictionary<int, string>();

            // Create a directory if it doesn't exist yet
            if (!Directory.Exists(DataPath))
                Directory.CreateDirectory(DataPath);

            string[] filePaths = Directory.GetFiles(DataPath);
            string[] savePaths = filePaths.Where(path => path.EndsWith(fileExtentionName)).ToArray();

            int pathCount = savePaths.Length;
            for (int i = 0; i < pathCount; i++)
            {
                string fileName = savePaths[i].Substring(DataPath.Length + gameFileName.Length + 1);
                if (
                    int.TryParse(
                        fileName.Substring(0, fileName.LastIndexOf(".", StringComparison.Ordinal)),
                        out int getSlotNumber
                    ))
                {
                    Log($"Found save file at: {savePaths[i]}");
                    newSavePaths.Add(getSlotNumber, savePaths[i]);
                }
            }

            cachedSavePaths = newSavePaths;
            return newSavePaths;
        }

        private static SaveGame LoadSaveFromPath(string savePath)
        {
            string data = "";

            using (var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
            {
                data = reader.ReadString();
            }

            if (string.IsNullOrEmpty(data))
            {
                Log($"Save file empty: {savePath}. It will be automatically removed");
                File.Delete(savePath);
                return null;
            }

            // TODO: fix
            SaveGame getSave = JsonUtility.FromJson<SaveGame>(data);

            if (getSave != null)
            {
                getSave.OnLoad();
                return getSave;
            }
            else
            {
                Log($"Save file corrupted: {savePath}");
                return null;
            }
        }

        public static int[] GetUsedSlots()
        {
            int[] saves = new int[ObtainSavePaths().Count];

            int counter = 0;
            foreach (int item in ObtainSavePaths().Keys)
            {
                saves[counter] = item;
                counter++;
            }

            return saves;
        }

        public static int GetSaveSlotCount() => ObtainSavePaths().Count;

        public static SaveGame LoadSave(int slot, bool createIfEmpty = false)
        {
            if (slot < 0)
            {
                Debug.LogError("Attempted to load negative slot");
                return null;
            }

            if (ObtainSavePaths().TryGetValue(slot, out string savePath))
            {
                SaveGame saveGame = LoadSaveFromPath(savePath);

                if (saveGame == null)
                {
                    ObtainSavePaths().Remove(slot);
                    return null;
                }

                Log($"Successful load at slot (from cache): {slot}");
                return saveGame;
            }
            else
            {
                if (!createIfEmpty)
                {
                    Log($"Could not load game at slot {slot}");
                }
                else
                {
                    Log($"Creating save at slot {slot}");

                    SaveGame saveGame = new SaveGame();

                    WriteSave(saveGame, slot);

                    return saveGame;
                }

                return null;
            }
        }

        public static void WriteSave(SaveGame saveGame, int saveSlot)
        {
            string savePath = $"{DataPath}/{gameFileName}{saveSlot.ToString()}{fileExtentionName}";

            if (!ObtainSavePaths().ContainsKey(saveSlot))
            {
                ObtainSavePaths().Add(saveSlot, savePath);
            }

            Log($"Saving game slot {saveSlot.ToString()} to : {savePath}");

            saveGame.OnWrite();

            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Converters.Add(new Vector3JsonConverter());
            serializer.Formatting = Formatting.Indented;
            using (StreamWriter sw = new StreamWriter(File.Open(savePath, FileMode.Create)))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, saveGame);
            }
        }

        public static void DeleteSave(int slot)
        {
            string filePath = $"{DataPath}/{gameFileName}{slot}{fileExtentionName}";

            if (File.Exists(filePath))
            {
                Log($"Successfully removed file at {filePath}");
                File.Delete(filePath);

                if (ObtainSavePaths().ContainsKey(slot))
                {
                    ObtainSavePaths().Remove(slot);
                }
            }
            else
            {
                Log($"Failed to remove file at {filePath}");
            }
        }

        public static bool IsSlotUsed(int index)
        {
            return ObtainSavePaths().ContainsKey(index);
        }

        public static int GetAvailableSaveSlot()
        {
            int slotCount = SaveSettingSO.Get().maxSaveSlotCount;

            for (int i = 0; i < slotCount; i++)
            {
                if (!ObtainSavePaths().ContainsKey(i))
                    return i;
            }

            return -1;
        }
    }
}