using System;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Block
{
    [CreateAssetMenu(menuName = "Game/BlockConfigSO")]
    public class BlockConfigSO : ScriptableObject
    {
        [SerializeField] private List<BlockConfig> _blockConfigs;

        public GameObject GetPrefab(BlockType type) => Search(type).prefab;
        public Material GetBuildModeMaterial(BlockType type) => Search(type).BuildModeMaterial;
        public Material GetDimmedMaterial(BlockType type) => Search(type).DimmedMaterial;
        public bool IsTwoClickBuild(BlockType type) => type == BlockType.Beam;
        public bool IsSingleClickBuild(BlockType type) => type == BlockType.Ball;

        private BlockConfig Search(BlockType type)
        {
            if (type == BlockType.None)
                throw new Exception($"{type} selected you pig");

            foreach (var e in _blockConfigs)
            {
                if (e.type == type)
                    return e;
            }

            throw new Exception($"Cannot find a block prefab of {type}, set it in the BlockConfigSO asset you pig");
        }
    }

    [Serializable]
    class BlockConfig
    {
        public BlockType type;
        public GameObject prefab;
        public Material BuildModeMaterial;
        public Material DimmedMaterial;
    }
}