using System;
using UnityEngine;

namespace Block
{
    [Serializable]
    public class HingeBallBeamConnection : BallBeamConnection
    {
        [SerializeField] private Vector3 axis = Vector3.up;

        private HingeJoint _joint = null;

        public HingeBallBeamConnection(TheBall ball, Beam beam) : base(ball, beam)
        {
        }

        public override Joint CreatePhysicalConnection()
        {
            _joint = _ball.gameObject.AddComponent<HingeJoint>();
            _joint.connectedBody = _beam.GetComponent<Rigidbody>();
            _joint.axis = axis;
            _joint.anchor = Vector3.zero;
            return _joint;
        }

        public override void DestroyPhysicalConnection()
        {
            GameObject.Destroy(_joint);
        }

        /// <summary>
        /// Change the axis of rotation of the hinge connection
        /// </summary>
        /// <param name="axis">Axis in local space of the object that owns the joint</param>
        public void SetRotationAxis(Vector3 axis)
        {
            _joint.axis = axis;
        }

        public override void OnDrawGizmos()
        {
            // draw rotation axis
            Gizmos.color = Color.cyan;
            var t = _ball.transform;
            Gizmos.DrawLine(t.position, t.position + t.TransformDirection(axis).normalized);
        }
    }
}