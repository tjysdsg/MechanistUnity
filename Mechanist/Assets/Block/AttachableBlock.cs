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

    public class AttachableBlock : BaseBlock
    {
        [SerializeField] protected List<Rigidbody> connectedRigidbodies = new List<Rigidbody>();

        public override void Initialize()
        {
            base.Initialize();

            foreach (var body in connectedRigidbodies)
            {
                AttachRigidBody(body);
            }
        }

        protected virtual void AttachRigidBody(Rigidbody body)
        {
        }

        public virtual void OnAttach(BlockAttachment attachment)
        {
            Rigidbody body = attachment.obj.GetComponent<Rigidbody>();
            AttachRigidBody(body);
            connectedRigidbodies.Add(body);
        }
    }
}