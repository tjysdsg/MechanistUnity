#nullable enable
using System.Collections.Generic;
using Core;
using UnityEngine.Assertions;

namespace Block
{
    public class TheBall : AttachableBlock
    {
        /// <summary>
        /// Map a beam to its connection index, so that we can quickly find out which connection you are editing when
        /// you clicked on a beam
        /// </summary>
        private Dictionary<BaseBlock, int> _beam2Connection = new Dictionary<BaseBlock, int>();

        public override BlockType GetBlockType() => BlockType.Ball;
        public override bool HasInterBlockCollision() => true;

        protected override void Initialize()
        {
        }

        protected override void LateInitialize()
        {
        }

        public override void OnAttach(BlockAttachment attachment)
        {
            Assert.IsTrue(attachment.obj is Beam);
            connections.Add(new FixBallBeamConnection(this, (Beam)attachment.obj));
        }

        public override BallBeamConnection? FindConnectionFromOther(BaseBlock other)
        {
            if (_beam2Connection.TryGetValue(other, out int i))
                return connections[i];

            return null;
        }

        private void OnDrawGizmos()
        {
            foreach (var conn in connections)
            {
                conn.OnDrawGizmos();
            }
        }
    }
}