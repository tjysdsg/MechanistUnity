using UnityEngine;

namespace Block
{
    public interface IJointConnection
    {
        public Joint CreatePhysicalConnection();

        public void DestroyPhysicalConnection();
    }

    public abstract class BallBeamConnection : IJointConnection
    {
        protected TheBall _ball = null;
        protected Beam _beam = null;

        public abstract Joint CreatePhysicalConnection();
        public abstract void DestroyPhysicalConnection();

        public BallBeamConnection(TheBall ball, Beam beam)
        {
            _ball = ball;
            _beam = beam;
        }
    }

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
    }
}