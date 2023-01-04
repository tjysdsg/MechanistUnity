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
        private Dictionary<Beam, int> _beam2Connection;

        public override void OnAttach(BlockAttachment attachment)
        {
            Assert.IsTrue(attachment.obj is Beam);
            connections.Add(new FixBallBeamConnection(this, (Beam)attachment.obj));
        }

        public BallBeamConnection GetConnectionOfBeam(Beam beam)
        {
            int i = _beam2Connection[beam];
            return connections[i];
        }

        private void OnDrawGizmos()
        {
            foreach (var conn in connections)
            {
                conn.OnDrawGizmos();
            }
        }

        public override BlockType GetBlockType()
        {
            return BlockType.Ball;
        }
    }
}