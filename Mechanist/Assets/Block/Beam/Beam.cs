using Core;
using UnityEngine;
using MeshUtils;
using UnityEngine.Assertions;

namespace Block
{
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Beam : TwoClickBuildBlock
    {
        private ProceduralCylinderMesh _proceduralMesh = null;

        [Header("Initial configs")] //
        [SerializeField]
        private int nRadialSegments = 10;

        [SerializeField] private int nHeightSegments = 2;
        [SerializeField] private float topRadius = 0.5f;
        [SerializeField] private float bottomRadius = 0.5f;
        [SerializeField] protected float length = 1;

        private Vector3? _block1Pos = null;
        private Vector3? _block2Pos = null;

        public override bool IsBlockAttachment() => true;

        protected override void OnEnterPlayMode()
        {
            gameObject.layer = ObjectLayer.GetBlockAttachmentLayerIndex();
        }

        protected override void OnEnterBuildMode()
        {
            gameObject.layer = ObjectLayer.GetBlockAttachmentLayerIndex();
        }

        public override BlockType GetBlockType() => BlockType.Beam;
        public override bool HasInterBlockCollision() => false;

        protected override void Initialize()
        {
        }

        protected override void LateInitialize()
        {
            // NOTE: must be in LateInitialize since we need to wait for somebody to assign block1 and block2
            Assert.IsTrue(block1 != null && block2 != null);

            AttachSelfToBlockIfHavent(block1);
            AttachSelfToBlockIfHavent(block2);

            UpdateProceduralModel();
        }

        protected override void Update()
        {
            base.Update();

            if (!_block1Pos.HasValue)
                _block1Pos = block1.position;
            if (!_block2Pos.HasValue)
                _block2Pos = block2.position;

            // update the model to always connect two blocks in build mode
            if (_gameMode == GameMode.BuildMode)
            {
                var p1 = block1!.position;
                var p2 = block2!.position;

                if (p1 != _block1Pos! || p2 != _block2Pos!)
                {
                    UpdateProceduralModel();
                    _block1Pos = p1;
                    _block2Pos = p2;
                }
            }
        }

        public override Rigidbody GetPlugForAttachedBlock(SingleClickBuildBlock scbb) => _rigidbody;

        /// <summary>
        /// Attach self to an attachable block if haven't.
        /// This is used for to insert missing connections to the attachable block during game initialization.
        /// No need to call this when entering play mode from build mode since <see cref="TheBall.OnAttach"/>
        /// would have been called by then.
        /// </summary>
        private void AttachSelfToBlockIfHavent(Transform b)
        {
            TheBall ab = b.GetComponent<TheBall>();
            if (ab.FindConnectionIndexFromOther(this) == -1)
                ab.OnAttach(new BlockAttachment(this));
        }

        protected (Vector3, Vector3) CalculatePositionAndDirectionVectors()
        {
            Vector3 pos1 = block1.position;
            Vector3 pos2 = block2.position;

            Vector3 position = (pos1 + pos2) / 2;
            Vector3 direction = pos1 - pos2;

            return (position, direction);
        }

        // Update the position and the mesh of the cylinder based on the two connected objects
        public virtual void UpdateProceduralModel()
        {
            if (_proceduralMesh == null)
                _proceduralMesh =
                    new ProceduralCylinderMesh(topRadius, bottomRadius, length, nRadialSegments, nHeightSegments);
            (Vector3 center, Vector3 direction) = CalculatePositionAndDirectionVectors();

            // update transform
            transform.SetPositionAndRotation(center, Quaternion.FromToRotation(Vector3.forward, direction));

            // update mesh
            length = direction.magnitude;
            GetComponent<MeshFilter>().sharedMesh = _proceduralMesh.UpdateMesh(topRadius, bottomRadius, length);
            GetComponent<MeshCollider>().sharedMesh = _proceduralMesh.Mesh;
        }

        #region Editor related

        private void OnValidate()
        {
            if (block1 != null && block2 != null)
            {
                (Vector3 center, Vector3 direction) = CalculatePositionAndDirectionVectors();
                transform.SetPositionAndRotation(center, Quaternion.FromToRotation(Vector3.forward, direction));
            }
        }

        #endregion
    }
}