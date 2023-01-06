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
        public Mesh Mesh => _proceduralCylinderMesh.Mesh;

        private ProceduralCylinderMesh _proceduralCylinderMesh = null;
        private MeshFilter _meshFilter = null;

        [Header("Initial configs")] public int nRadialSegments = 10;
        public int nHeightSegments = 2;
        public float topRadius = 0.5f;
        public float bottomRadius = 0.5f;
        [SerializeField] private float length = 1;

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

        public override void Initialize()
        {
            base.Initialize();

            _meshCollider = GetComponent<MeshCollider>();

            _meshFilter = GetComponent<MeshFilter>();

            if (_proceduralCylinderMesh == null)
                _proceduralCylinderMesh =
                    new ProceduralCylinderMesh(topRadius, bottomRadius, length, nRadialSegments, nHeightSegments);

            UpdateProceduralModel();
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
            if (block1 == null || block2 == null)
            {
                return;
            }

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
            ProceduralCylinderMesh mesh =
                new ProceduralCylinderMesh(topRadius, bottomRadius, length, nRadialSegments, nHeightSegments);
            GetComponent<MeshFilter>().sharedMesh = mesh.UpdateMesh();
        }

        public void OnDrawGizmos()
        {
            if (block1 == null || block2 == null)
            {
                return;
            }

            var gizmoMesh = new ProceduralCylinderMesh(0.2f, 0.2f, 1, 4, 1);

            // draw gizmo mesh
            Gizmos.color = Color.gray;
            (Vector3 center, Vector3 direction) = CalculatePositionAndDirectionVectors();
            Gizmos.DrawMesh(gizmoMesh.UpdateMesh(0.2f, 0.2f, direction.magnitude), center,
                Quaternion.FromToRotation(Vector3.forward, direction));
        }

        #endregion
    }
}