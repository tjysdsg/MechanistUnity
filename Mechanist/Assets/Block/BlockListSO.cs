using System;
using System.Collections.Generic;
using UnityEngine;

namespace Block
{
    /// <summary>
    /// We DO NOT want it to be serializable so it won't be saved to disk
    /// </summary>
    [CreateAssetMenu(menuName = "Game/BlockListSO")]
    public class BlockListSO : ScriptableObject
    {
        [NonSerialized] public HashSet<BaseBlock> blocks = new HashSet<BaseBlock>();
    }
}