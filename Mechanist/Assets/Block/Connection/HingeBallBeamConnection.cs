using System;
using Core;
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

        public HingeBallBeamConnection(TheBall ball, Beam beam, Rigidbody plug, GameObject prefab)
            : base(ball, beam, plug)
        {
            _prefab = prefab;
        }

        public override Joint CreatePhysicalConnection()
        {
            _joint = _ball.gameObject.AddComponent<HingeJoint>();
            _joint.connectedBody = _plug;
            _joint.axis = axis;
            _joint.anchor = Vector3.zero;
            return _joint;
        }

        public override void DestroyPhysicalConnection()
        {
            GameObject.Destroy(_joint);
        }

        public override void OnDisable()
        {
            if (_connectionModel is not null)
                GameObject.Destroy(_connectionModel);
        }

        public override void Update()
        {
            if (_connectionModel is null)
                _connectionModel = GameObject.Instantiate(_prefab);

            Vector3 direction = _beam.transform.position - _ball.transform.position;
            Vector3 axisWorld = _ball.transform.TransformDirection(axis);

            Vector3 upwards = Vector3.Cross(axisWorld, direction);
            Vector3 forward = Vector3.Cross(upwards, axisWorld);
            Quaternion rotation = Quaternion.LookRotation(forward, upwards);
            _connectionModel.transform.SetPositionAndRotation(_ball.transform.position, rotation);
        }

        public void SetAxis(Vector3 axis, Space space)
        {
            if (space == Space.Self)
                this.axis = axis.normalized;
            else
                this.axis = _ball.transform.InverseTransformDirection(axis).normalized;
        }

        public override void OnDrawGizmos()
        {
            // draw rotation axis
            Gizmos.color = Color.cyan;
            var t = _ball.transform;
            Gizmos.DrawLine(t.position, t.position + t.TransformDirection(axis).normalized);
        }

        public override BlockConnectionType GetConnectionType() => BlockConnectionType.Hinge;
    }
}