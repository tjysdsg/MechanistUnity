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
    public abstract class AttachableBlock : BaseBlock
    {
        [SerializeField] protected List<Rigidbody> connectedRigidbodies = default;

        /// <summary>
        /// Enter game play mode, should create relevant components such as joints)
        /// </summary>
        public abstract void EnterPlayMode();

        public virtual void OnAttach(BlockAttachment attachment)
        {
            Rigidbody body = attachment.obj.GetComponent<Rigidbody>();
            connectedRigidbodies.Add(body);
        }
    }
}