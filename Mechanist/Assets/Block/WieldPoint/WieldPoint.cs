using System.Collections.Generic;
using UnityEngine;

namespace Block
{
    public class WieldPoint : AttachableBlock
    {
        [SerializeField] private List<Rigidbody> connectedRigidbodies = new List<Rigidbody>();

        public override void OnAttach(BlockAttachment attachment)
        {
            FixedJoint joint = _go.AddComponent<FixedJoint>();
            Rigidbody body = attachment.obj.GetComponent<Rigidbody>();
            joint.connectedBody = body;
            connectedRigidbodies.Add(body);
        }
    }
}