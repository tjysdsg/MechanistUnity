using System;
using Core;
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
        public TheBall Ball => _ball;
        [SerializeField] protected Beam _beam = null;
        public Beam Beam => _beam;
        [SerializeField] protected Rigidbody _plug = null;
        public Rigidbody Plug => _plug;

        public abstract Joint CreatePhysicalConnection();
        public abstract void DestroyPhysicalConnection();
        public abstract void Update();

        public BallBeamConnection(TheBall ball, Beam beam, Rigidbody plug)
        {
            _ball = ball;
            _beam = beam;
            _plug = plug;
        }

        public virtual void OnDrawGizmos()
        {
        }

        public abstract BlockConnectionType GetConnectionType();
    }
}