using System;
using UnityEngine;

namespace Block
{
    [Serializable]
    public class FixBallBeamConnection : BallBeamConnection
    {
        private FixedJoint _joint = null;

        public FixBallBeamConnection(TheBall ball, Beam beam) : base(ball, beam)
        {
        }

        public override Joint CreatePhysicalConnection()
        {
            _joint = _ball.gameObject.AddComponent<FixedJoint>();
            _joint.connectedBody = _beam.GetComponent<Rigidbody>();
            return _joint;
        }

        public override void DestroyPhysicalConnection()
        {
            GameObject.Destroy(_joint);
        }

        public override void Update()
        {
        }
    }
}