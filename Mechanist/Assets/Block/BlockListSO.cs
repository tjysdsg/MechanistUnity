using System;
using System.Collections.Generic;
using SaveSystem;
using UnityEngine;

namespace Block
{
    /// <summary>
    /// We DO NOT want it to be serializable so it won't be saved to disk
    /// </summary>
    [CreateAssetMenu(menuName = "Game/BlockListSO")]
    public class BlockListSO : ScriptableObject, ISaveableInstanceLedger
    {
        [NonSerialized] public HashSet<BaseBlock> blocks = new HashSet<BaseBlock>();

        [SerializeField] private SaveSystemSO _saveSystem;

        public IEnumerable<ISaveable> GetSaveableInstances() => blocks;

        private void OnEnable()
        {
            _saveSystem.RegisterSaveableInstanceLedger(this);
        }
    }
}