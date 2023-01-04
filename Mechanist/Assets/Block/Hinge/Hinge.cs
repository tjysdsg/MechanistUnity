using UnityEngine.Assertions;

namespace Block
{
    public class Hinge : TheBall
    {
        public override void OnAttach(BlockAttachment attachment)
        {
            Assert.IsTrue(attachment.obj is Beam);
            connections.Add(new HingeBallBeamConnection(this, (Beam)attachment.obj));
        }
    }
}