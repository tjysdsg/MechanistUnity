using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using SaveSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace Block
{
    public class TheBall : SingleClickBuildBlock
    {
        [SerializeReference] private List<BallBeamConnection> _connections = new List<BallBeamConnection>();

        /// <summary>
        /// Map a block attachment to its connection index, so that we can quickly find out which connection you are
        /// editing when you clicked on an attachment
        /// </summary>
        private readonly Dictionary<Beam, int> _beamToConnection =
            new Dictionary<Beam, int>();

        public override BlockType GetBlockType() => BlockType.Ball;
        public override bool HasInterBlockCollision() => true;
        public override bool IsBlockAttachment() => false;

        protected override void Initialize()
        {
        }

        protected override void LateInitialize()
        {
        }

        protected override void Update()
        {
            base.Update();

            foreach (var conn in _connections)
            {
                conn.Update();
            }
        }

        public virtual void OnAttach(BlockAttachment attachment)
        {
            Assert.IsTrue(attachment.obj is Beam);

            var beam = (Beam)attachment.obj;
            _connections.Add(new FixBallBeamConnection(this, beam)); // fixed connection by default

            _beamToConnection[(Beam)attachment.obj] = _connections.Count - 1;
        }

        public int FindConnectionIndexFromOther(Beam other)
        {
            if (_beamToConnection.TryGetValue(other, out int i))
                return i;
            return -1;
        }

        public Beam GetConnectedBeam(int connectionIndex)
        {
            Assert.AreNotEqual(-1, connectionIndex);
            return _connections.ElementAt(connectionIndex).Beam;
        }

        public BallBeamConnection GetConnectionAtIndex(int connectionIndex) => _connections[connectionIndex];

        public void SetConnectionAtIndex(int connectionIndex, BallBeamConnection conn)
        {
            Assert.IsFalse(_connections.Contains(conn));
            _connections[connectionIndex] = conn;
        }

        protected override void OnEnterPlayMode()
        {
            foreach (var conn in _connections)
            {
                conn.CreatePhysicalConnection();
            }
        }

        protected override void OnEnterBuildMode()
        {
            foreach (var conn in _connections)
            {
                conn.DestroyPhysicalConnection();
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var conn in _connections)
            {
                conn.OnDrawGizmos();
            }
        }

        #region Save and load

        [Serializable]
        internal class BallSaveData : SaveData
        {
            public Vector3 position;
            public List<SaveData> connections;

            public BallSaveData(int id, string typename) : base(id, typename)
            {
            }
        }

        public override void OnLoad(SaveData data, ISaveableInstanceLoader loader)
        {
            // TODO: Implement this
            throw new System.NotImplementedException();
        }

        public override SaveData OnSave()
        {
            var data = new BallSaveData(GetSaveDataId(), GetBlockType().ToString());
            data.position = transform.position;
            data.connections = new();

            foreach (var conn in _connections)
                data.connections.Add(conn.OnSave());

            return data;
        }

        #endregion
    }
}