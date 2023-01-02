using UnityEngine;

namespace Block
{
    public class WieldPoint : AttachableBlock
    {
        protected override void AttachRigidBody(Rigidbody body)
        {
            FixedJoint joint = _go.AddComponent<FixedJoint>();
            joint.connectedBody = body;
        }
    }
}