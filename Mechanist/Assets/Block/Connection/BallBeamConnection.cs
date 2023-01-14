using System;
using Core;
using SaveSystem;
using UnityEngine;

namespace Block
{
    public interface IJointConnection
    {
        public Joint CreatePhysicalConnection();

        public void DestroyPhysicalConnection();
    }

    [Serializable]
    public abstract class BallBeamConnection : IJointConnection, ISaveable
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
        public abstract void OnDisable();

        public BallBeamConnection(TheBall ball, Beam beam)
        {
            _ball = ball;
            _beam = beam;
            _plug = beam.GetPlugForAttachedBlock(ball);
        }

        public virtual void OnDrawGizmos()
        {
        }

        public abstract BlockConnectionType GetConnectionType();

        public int GetSaveDataId() => 0;
        public abstract SaveData OnSave();
        public abstract void OnLoad(SaveData data, ISaveableInstanceLoader loader);
    }
}