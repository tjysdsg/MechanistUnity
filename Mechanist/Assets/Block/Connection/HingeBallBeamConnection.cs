using System;
using UnityEngine;

namespace Block
{
    [Serializable]
    public class HingeBallBeamConnection : BallBeamConnection
    {
        [SerializeField] private Vector3 axis = Vector3.up;

        private HingeJoint _joint = null;
        private GameObject _connectionModel = null;
        private GameObject _prefab = null;

        public HingeBallBeamConnection(TheBall ball, Beam beam, GameObject prefab) : base(ball, beam)
        {
            _prefab = prefab;
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

        public override void Update()
        {
            if (_connectionModel == null)
            {
                _connectionModel = GameObject.Instantiate(_prefab);
            }

            Vector3 direction = _beam.transform.position - _ball.transform.position;
            Quaternion rotation = Quaternion.LookRotation(
                direction,
                Vector3.Cross(_ball.transform.TransformDirection(axis), direction)
            );
            _connectionModel.transform.SetPositionAndRotation(_ball.transform.position, rotation);
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