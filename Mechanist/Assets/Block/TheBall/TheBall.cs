using System.Collections.Generic;
using Core;
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

        protected override void Initialize()
        {
        }

        protected override void LateInitialize()
        {
        }

        public virtual void OnAttach(BlockAttachment attachment)
        {
            Assert.IsTrue(attachment.obj is Beam);
            _connections.Add(new FixBallBeamConnection(this, (Beam)attachment.obj)); // fixed connection by default
            _beamToConnection[(Beam)attachment.obj] = _connections.Count - 1;
        }

        public int FindConnectionIndexFromOther(Beam other)
        {
            if (_beamToConnection.TryGetValue(other, out int i))
                return i;
            return -1;
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
    }
}