using System;
using UnityEngine;

namespace Block
{
    [Serializable]
    public struct BlockAttachment
    {
        public BaseBlock obj;
        public Vector3 point;
    }

    public class AttachableBlock : BaseBlock
    {
        public virtual void OnAttach(BlockAttachment attachment)
        {
        }
    }
}