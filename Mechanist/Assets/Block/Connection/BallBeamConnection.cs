using System;
using UnityEngine;

namespace Block
{
    public interface IJointConnection
    {
        public Joint CreatePhysicalConnection();

        public void DestroyPhysicalConnection();
    }

    [Serializable]
    public abstract class BallBeamConnection : IJointConnection
    {
        [SerializeField] protected TheBall _ball = null;
        [SerializeField] protected Beam _beam = null;

        public abstract Joint CreatePhysicalConnection();
        public abstract void DestroyPhysicalConnection();

        public BallBeamConnection(TheBall ball, Beam beam)
        {
            _ball = ball;
            _beam = beam;
        }

        public virtual void OnDrawGizmos()
        {
        }
    }
}