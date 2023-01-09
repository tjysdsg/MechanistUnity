using Core;
using UnityEngine;
using MeshUtils;

namespace Block
{
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Beam : TwoClickBuildBlock
    {
        private MeshCollider _meshCollider = null;
        private Mesh Mesh => _proceduralCylinderMesh.Mesh;

        private ProceduralCylinderMesh _proceduralCylinderMesh = null;
        private MeshFilter _meshFilter = null;

        [Header("Initial configs")] public int nRadialSegments = 10;
        public int nHeightSegments = 2;
        public float topRadius = 0.5f;
        public float bottomRadius = 0.5f;
        [SerializeField] private float length = 1;

        private Vector3 _block1Pos;
        private Vector3 _block2Pos;

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
            _meshCollider = GetComponent<MeshCollider>();
            _meshFilter = GetComponent<MeshFilter>();

            _block1Pos = block1.position;
            _block2Pos = block2.position;
        }

        protected override void Update()
        {
            base.Update();

            // update the model to always connect two blocks in build mode
            if (_gameMode == GameMode.BuildMode)
            {
                var p1 = block1.position;
                var p2 = block2.position;

                if (p1 != _block1Pos || p2 != _block2Pos)
                {
                    UpdateProceduralModel();
                    _block1Pos = p1;
                    _block2Pos = p2;
                }
            }
        }

        protected override void LateInitialize()
        {
            if (block1 == null || block2 == null)
                return;

            AttachSelfToBlockIfHavent(block1);
            AttachSelfToBlockIfHavent(block2);

            _proceduralCylinderMesh =
                new ProceduralCylinderMesh(topRadius, bottomRadius, length, nRadialSegments, nHeightSegments);
            UpdateProceduralModel();
        }

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
            {
                ab.OnAttach(new BlockAttachment
                {
                    obj = this,
                    point = transform.position,
                });
            }
        }

        private (Vector3, Vector3) CalculatePositionAndDirectionVectors()
        {
            Vector3 pos1 = block1.position;
            Vector3 pos2 = block2.position;

            Vector3 position = (pos1 + pos2) / 2;
            Vector3 direction = pos1 - pos2;

            return (position, direction);
        }

        // Update the position and the mesh of the cylinder based on the two connected objects
        public void UpdateProceduralModel()
        {
            (Vector3 center, Vector3 direction) = CalculatePositionAndDirectionVectors();

            // update transform
            transform.SetPositionAndRotation(center, Quaternion.FromToRotation(Vector3.forward, direction));

            // generate/update cylinder mesh
            length = direction.magnitude;
            _meshFilter.sharedMesh = _proceduralCylinderMesh.UpdateMesh(topRadius, bottomRadius, length);
            _meshCollider.sharedMesh = Mesh;
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

        public void UpdateMeshInEditor()
        {
            if (block1 == null || block2 == null)
                return;

            (Vector3 center, Vector3 direction) = CalculatePositionAndDirectionVectors();

            // update transform
            transform.SetPositionAndRotation(center, Quaternion.FromToRotation(Vector3.forward, direction));

            // generate/update cylinder mesh
            length = direction.magnitude;
            ProceduralCylinderMesh mesh =
                new ProceduralCylinderMesh(topRadius, bottomRadius, length, nRadialSegments, nHeightSegments);
            GetComponent<MeshFilter>().sharedMesh = mesh.UpdateMesh();
            GetComponent<MeshCollider>().sharedMesh = mesh.Mesh;
        }

        #endregion
    }
}