﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Block
{
    [CreateAssetMenu(menuName = "Game/BlockTypeSO")]
    public class BlockTypeSO : ScriptableObject
    {
        public BlockType type = BlockType.None;

        [SerializeField] private List<BlockTypeToPrefab> _blockTypePrefabs;

        public GameObject GetPrefab()
        {
            if (type == BlockType.None)
                throw new Exception($"{type} selected you pig");

            GameObject ret = null;
            foreach (var e in _blockTypePrefabs)
            {
                if (e.type == type)
                {
                    ret = e.prefab;
                    break;
                }
            }

            if (ret == null)
                throw new Exception($"Cannot find a block prefab of {type}, set it in the BlockTypeSO asset you pig");
            return ret;
        }

        public bool IsTwoClickBuild() => type == BlockType.Brace;

        public bool IsSingleClickBuild() => type is BlockType.Hinge or BlockType.WieldPoint;

        public bool IsNone() => type == BlockType.None;
    }

    /// <summary>
    /// Use this instead of a Dictionary<BlockType, GameObject> so that you can edit the mapping in Unity editor
    /// </summary>
    [Serializable]
    class BlockTypeToPrefab
    {
        public BlockType type;
        public GameObject prefab;
    }
}