using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
        private List<Joint> _joints;

        public override void EnterPlayMode()
        {
            Assert.AreEqual(0, _joints.Count);
            foreach (var body in connectedRigidbodies)
            {
                FixedJoint joint = _go.AddComponent<FixedJoint>();
                joint.connectedBody = body;
                _joints.Add(joint);
            }
        }

        // TODO: reset position
        public override void EnterBuildMode()
        {
            foreach (var joint in _joints)
            {
                Destroy(joint);
            }

            _joints.Clear();
        }
    }
}