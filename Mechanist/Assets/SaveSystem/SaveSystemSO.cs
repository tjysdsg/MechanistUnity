using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SaveSystem
{
    [CreateAssetMenu(menuName = "SaveSystem/SaveSystemSO")]
    public class SaveSystemSO : ScriptableObject
    {
        private SaveGame _saveGame = new SaveGame();

        private readonly Dictionary<string, ISaveableInstanceLoader> _saveableInstanceLoaders = new();
        private readonly List<ISaveableInstanceLedger> _saveableInstanceLedgers = new();

        /**
         * <summary>
         * Save all <see cref="ISaveable"/> instances registered by <see cref="SaveSystemSO.RegisterSaveableInstanceLedger"/>.
         * </summary>
         */
        public void Save(int saveSlot)
        {
            _saveGame.Clear();

            foreach (var ledger in _saveableInstanceLedgers)
            {
                var saveables = ledger.GetSaveableInstances();
                foreach (var s in saveables)
                    _saveGame.AddData(s.OnSave());
            }

            SaveFileUtils.WriteSave(_saveGame, saveSlot);
        }

        public void Load(int saveSlot)
        {
            _saveGame = SaveFileUtils.LoadSave(saveSlot);

            foreach (ISaveableInstanceLoader loader in _saveableInstanceLoaders.Values)
            {
                loader.Clear();
            }

            foreach (SaveData data in _saveGame)
            {
                // TODO: show error to user
                if (_saveableInstanceLoaders.TryGetValue(data.typename, out ISaveableInstanceLoader loader))
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
            var ids = loader.GetHandledTypes();
            foreach (var id in ids)
            {
                Assert.IsFalse(_saveableInstanceLoaders.ContainsKey(id));
                _saveableInstanceLoaders[id] = loader;
            }
        }

        /**
         * <summary>
         * Register an <see cref="ISaveableInstanceLedger"/>, all saveable objects returned by
         * <see cref="ISaveableInstanceLedger.GetSaveableInstances"/> will be written to file when 
         * <see cref="SaveSystemSO.Save"/> is called.
         * </summary>
         */
        public void RegisterSaveableInstanceLedger(ISaveableInstanceLedger ledger)
        {
            _saveableInstanceLedgers.Add(ledger);
        }
    }
}