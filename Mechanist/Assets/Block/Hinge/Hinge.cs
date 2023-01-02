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
        private Joint _joint = default;

        public override void OnAttach(BlockAttachment attachment)
        {
            Assert.IsTrue(connectedRigidbodies.Count < 2);

            Transform t = attachment.obj.transform;
            Rigidbody body = t.GetComponent<Rigidbody>();

            connectedRigidbodies.Add(body);
        }

        public override void EnterPlayMode()
        {
            var go = connectedRigidbodies[0].gameObject;
            _joint = go.AddComponent<HingeJoint>();
            _joint.connectedBody = connectedRigidbodies[1].GetComponent<Rigidbody>();
            _joint.anchor = go.transform.InverseTransformPoint(transform.TransformPoint(Vector3.zero));
            _joint.axis = go.transform.InverseTransformDirection(transform.TransformDirection(Vector3.up));
        }

        // TODO: reset position
        public override void EnterBuildMode()
        {
            Destroy(_joint);
        }
    }
}