using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Block
{
    public class TheBall : AttachableBlock
    {
        // TODO: move this into AttachableBlock
        private List<BallBeamConnection> _connections = new List<BallBeamConnection>();

        /// <summary>
        /// Map a beam to its connection index, so that we can quickly find out which connection you are editing when
        /// you clicked on a beam
        /// </summary>
        private Dictionary<Beam, int> _beam2Connection;

        public override void OnAttach(BlockAttachment attachment)
        {
            base.OnAttach(attachment);

            Assert.IsTrue(attachment.obj is Beam);
            _connections.Add(new FixBallBeamConnection(this, (Beam)attachment.obj));
        }

        // public void ChangeConnectionType(Beam beam)
        // {
        //     int i = _beam2Connection[beam];
        //     _connections[i].xxx = xxx;
        // }

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
    }
}