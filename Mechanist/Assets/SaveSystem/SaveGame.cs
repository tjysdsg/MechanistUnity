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

    /// <summary>
    /// The data of a single game object, a component, etc.
    /// This is the base class. It's intended to be inherited to add custom serializable fields.
    /// </summary>
    /// 
    /// <remarks>
    /// "Reference" is a special type of save data.
    /// It is used for saving game objects that contains references to other game objects
    /// without duplicating data.
    /// This can ensure proper loading.
    ///
    /// In this case,
    /// The <see cref="SaveData.id"/> field contains the ID of the referred object.
    /// </remarks>
    [Serializable]
    public abstract class SaveData
    {
        /// <summary>
        /// Unique ID
        /// </summary>
        public int id;

        /// <summary>
        /// Type of this game object/component/...
        ///
        /// A corresponding <see cref="ISaveableInstanceLoader"/> will be called to load this data.
        /// </summary>
        public string typename;

        public SaveData(int id, string typename)
        {
            this.id = id;
            this.typename = typename;
        }
    }

    public class ReferenceSaveData : SaveData
    {
        public ReferenceSaveData(int id) : base(id, "Reference")
        {
        }
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
        [SerializeReference] private List<SaveData> _saveData = new List<SaveData>();

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

        public void AddData(SaveData data) => _saveData.Add(data);
        public void Clear() => _saveData.Clear();
    }
}