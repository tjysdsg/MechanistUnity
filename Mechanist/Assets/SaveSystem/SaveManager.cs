using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SaveSystem
{
    public class SaveManager : ScriptableObject
    {
        private SaveGame _saveGame = new SaveGame();

        private Dictionary<string, ISaveableInstanceLoader> _saveableInstanceLoaders = new();
        private List<ISaveableInstanceLedger> _saveableInstanceLedgers = new();

        public void Save(int saveSlot)
        {
            _saveGame.Clear();

            foreach (var ledger in _saveableInstanceLedgers)
            {
                var saveables = ledger.GetSaveableInstances();
                foreach (var s in saveables)
                    _saveGame.AddData(s.GetId(), s.OnSave());
            }

            FileSystemUtils.WriteSave(_saveGame, saveSlot);
        }

        public void Load(int saveSlot)
        {
            _saveGame = FileSystemUtils.LoadSave(saveSlot);

            foreach (SaveData data in _saveGame)
            {
                // TODO: show error to user
                if (_saveableInstanceLoaders.TryGetValue(data.id, out ISaveableInstanceLoader loader))
                {
                    if (!loader.Load(data))
                        Debug.LogError("Cannot load save data");
                }
                else
                {
                    Debug.LogError("Cannot load save data");
                }
            }
        }

        public void RegisterSaveableInstanceLoader(ISaveableInstanceLoader loader)
        {
            var ids = loader.GetHandledIds();
            foreach (var id in ids)
            {
                Assert.IsFalse(_saveableInstanceLoaders.ContainsKey(id));
                _saveableInstanceLoaders[id] = loader;
            }
        }

        public void RegisterSaveableInstanceLedger(ISaveableInstanceLedger ledger)
        {
            _saveableInstanceLedgers.Add(ledger);
        }
    }
}