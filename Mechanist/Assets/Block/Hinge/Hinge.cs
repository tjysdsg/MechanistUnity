using UnityEngine;
using UnityEngine.Assertions;

namespace Block
{
    /// <summary>
    /// A hinge block consists of two AttachableBlock:
    ///
    /// <list type="number">
    /// <item>The hinge itself, which requires 2 connections.</item>
    /// <item>A <see cref="WieldPoint"/>.</item>
    /// </list>
    ///
    /// However, this class only implements the first part.
    /// </summary>
    public class Hinge : AttachableBlock
    {
        protected override void AttachRigidBody(Rigidbody body)
        {
            Assert.IsTrue(connectedRigidbodies.Count < 2);
            base.AttachRigidBody(body);
        }

        public override void EnterPlayMode()
        {
            var go = connectedRigidbodies[0].gameObject;
            HingeJoint joint = go.AddComponent<HingeJoint>();
            joint.connectedBody = connectedRigidbodies[1].GetComponent<Rigidbody>();
            joint.anchor = go.transform.InverseTransformPoint(transform.TransformPoint(Vector3.zero));
            joint.axis = go.transform.InverseTransformDirection(transform.TransformDirection(Vector3.up));
        }
    }
}