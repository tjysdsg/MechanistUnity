using UnityEngine;
using UnityEngine.Assertions;

namespace Block
{
    public class Hinge : TheBall
    {
        private HingeJoint _joint = null;

        public override void OnAttach(BlockAttachment attachment)
        {
            Assert.IsTrue(attachment.obj is Beam);
            connections.Add(new HingeBallBeamConnection(this, (Beam)attachment.obj));
        }
    }
}