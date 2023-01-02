using UnityEngine;

namespace Block
{
    public class Hinge : AttachableBlock
    {
        protected override void AttachRigidBody(Rigidbody body)
        {
            var attachedGo = body.gameObject;
            HingeJoint joint = attachedGo.AddComponent<HingeJoint>();
            joint.connectedBody = _go.GetComponent<Rigidbody>();
            joint.anchor = attachedGo.transform.InverseTransformPoint(transform.TransformPoint(Vector3.zero));
            joint.axis = attachedGo.transform.InverseTransformDirection(transform.TransformDirection(Vector3.up));
        }
    }
}