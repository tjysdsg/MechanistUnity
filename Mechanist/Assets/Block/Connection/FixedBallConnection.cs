using System;
using Core;
using UnityEngine;

namespace Block
{
    [Serializable]
    public class FixBallBeamConnection : BallBeamConnection
    {
        private FixedJoint _joint = null;

        public FixBallBeamConnection(TheBall ball, Beam beam, Rigidbody plug) : base(ball, beam, plug)
        {
        }

        public override Joint CreatePhysicalConnection()
        {
            _joint = _ball.gameObject.AddComponent<FixedJoint>();
            _joint.connectedBody = _plug;
            return _joint;
        }

        public override void DestroyPhysicalConnection()
        {
            GameObject.Destroy(_joint);
        }

        public override void Update()
        {
        }

        public override BlockConnectionType GetConnectionType() => BlockConnectionType.Fixed;
    }
}