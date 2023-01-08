using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace BuildMode
{
    [CreateAssetMenu(menuName = "Game/BlockConnectionConfigSO")]
    public class BlockConnectionConfigSO : ScriptableObject
    {
        [SerializeField] private List<BlockConnectionConfig> _configs;

        public GameObject GetPrefab(BlockConnectionType type) => Search(type).prefab;

        private BlockConnectionConfig Search(BlockConnectionType type)
        {
            foreach (var e in _configs)
            {
                if (e.type == type)
                    return e;
            }

            throw new Exception($"Cannot find a prefab for {type}, set it in the BlockConfigSO asset you pig");
        }
    }

    [Serializable]
    internal class BlockConnectionConfig
    {
        public BlockConnectionType type;
        public GameObject prefab;
    }
}