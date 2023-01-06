using System;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.Serialization;

namespace Block
{
    [CreateAssetMenu(menuName = "Game/BlockConfigSO")]
    public class BlockConfigSO : ScriptableObject
    {
        public BlockType type = BlockType.None;

        [FormerlySerializedAs("_blockTypePrefabs")] [SerializeField]
        private List<BlockConfig> _blockConfigs;

        public GameObject GetPrefab() => Search(type).prefab;
        public Material GetBuildModeMaterial(BlockType pType) => Search(pType).BuildModeMaterial;
        public Material GetDimmedMaterial(BlockType pType) => Search(pType).DimmedMaterial;

        private BlockConfig Search(BlockType pType)
        {
            if (pType == BlockType.None)
                throw new Exception($"{pType} selected you pig");

            foreach (var e in _blockConfigs)
            {
                if (e.type == pType)
                    return e;
            }

            throw new Exception($"Cannot find a block prefab of {pType}, set it in the BlockConfigSO asset you pig");
        }

        public bool IsTwoClickBuild() => type == BlockType.Beam;

        public bool IsSingleClickBuild() => type is BlockType.Hinge or BlockType.Ball;

        public bool IsNone() => type == BlockType.None;
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