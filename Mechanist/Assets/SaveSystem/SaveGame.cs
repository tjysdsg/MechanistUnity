using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public struct SaveGameMetaData
    {
        public int gameVersion;
        public string creationDate;
    }

    [Serializable]
    public struct SaveData
    {
        public string id;
        public string data;
    }

    /// <summary>
    /// Container for all information of a single save file.
    /// </summary>
    [Serializable]
    public class SaveGame : IEnumerable<SaveData>
    {
        [NonSerialized] public int gameVersion;
        [NonSerialized] public DateTime creationDate;

        [SerializeField] private SaveGameMetaData _metaData;
        [SerializeField] private List<SaveData> _saveData = new List<SaveData>();

        public IEnumerator<SaveData> GetEnumerator() => _saveData.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void OnWrite()
        {
            creationDate = DateTime.Now;
            _metaData.creationDate = creationDate.ToString(CultureInfo.InvariantCulture);
            _metaData.gameVersion = gameVersion;
        }

        public void OnLoad()
        {
            gameVersion = _metaData.gameVersion;
            if (!DateTime.TryParse(_metaData.creationDate, out creationDate))
            {
                Debug.LogWarning("Failed to parse save game creation date, using current date instead");
                creationDate = DateTime.Now;
            }

            _saveData.Clear();
        }

        public void AddData(string id, string data)
        {
            SaveData newSaveData = new SaveData { id = id, data = data };
            _saveData.Add(newSaveData);
        }

        public void Clear()
        {
            _saveData.Clear();
        }
    }
}