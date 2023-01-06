using System;
using System.Collections.Generic;
using UnityEngine;

namespace Block
{
    [Serializable]
    public struct BlockAttachment
    {
        public BaseBlock obj;
        public Vector3 point;
    }

    /// <summary>
    /// During build mode, this class only remembers which object(s) are connected.
    /// 
    /// During gameplay, we initialize the physics (creating relevant joints, rigidbodies, etc.) based on the specific
    /// type of block
    /// </summary>
    public abstract class AttachableBlock : SingleClickBuildBlock
    {
        [SerializeReference] protected List<BallBeamConnection> connections = new List<BallBeamConnection>();

        public abstract void OnAttach(BlockAttachment attachment);
        public abstract BallBeamConnection FindConnectionFromOther(BaseBlock other);

        protected override void OnEnterPlayMode()
        {
            foreach (var conn in connections)
            {
                conn.CreatePhysicalConnection();
            }
        }

        protected override void OnEnterBuildMode()
        {
            foreach (var conn in connections)
            {
                conn.DestroyPhysicalConnection();
            }
        }
    }
}