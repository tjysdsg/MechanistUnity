using UnityEngine;

namespace Block
{
    /// <summary>
    /// A wield point creates a FixedJoint to everything it is connected to.
    /// </summary>
    /// 
    /// <remarks>
    /// Creating a joint for each connection instead of using a "compound rigidbody" allows individual connection to
    /// be broken according to the physics engine.
    /// </remarks>
    public class WieldPoint : AttachableBlock
    {
        public override void EnterPlayMode()
        {
            foreach (var body in connectedRigidbodies)
            {
                FixedJoint joint = _go.AddComponent<FixedJoint>();
                joint.connectedBody = body;
            }
        }
    }
}