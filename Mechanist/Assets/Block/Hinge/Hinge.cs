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
        private HingeJoint _hingeJoint = null;
        private FixedJoint _fixedJoint = null;

        public override void OnAttach(BlockAttachment attachment)
        {
            Assert.IsTrue(connectedRigidbodies.Count < 2);

            Transform t = attachment.obj.transform;
            Rigidbody body = t.GetComponent<Rigidbody>();

            connectedRigidbodies.Add(body);
        }

        protected override void OnEnterPlayMode()
        {
            if (connectedRigidbodies.Count != 2) return;

            var go = connectedRigidbodies[0].gameObject;
            _hingeJoint = go.AddComponent<HingeJoint>();
            _hingeJoint.connectedBody = connectedRigidbodies[1].GetComponent<Rigidbody>();
            _hingeJoint.anchor = go.transform.InverseTransformPoint(transform.TransformPoint(Vector3.zero));
            _hingeJoint.axis = go.transform.InverseTransformDirection(transform.TransformDirection(Vector3.up));
            _hingeJoint.enableCollision = false;

            _fixedJoint = _go.AddComponent<FixedJoint>();
            _fixedJoint.connectedBody = connectedRigidbodies[0];
        }

        protected override void OnEnterBuildMode()
        {
            if (_hingeJoint != null)
                Destroy(_hingeJoint);
            if (_fixedJoint != null)
                Destroy(_fixedJoint);
        }
    }
}