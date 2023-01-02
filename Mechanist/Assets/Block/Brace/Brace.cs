using UnityEngine;
using MeshUtils;

namespace Block
{
    [RequireComponent(typeof(MeshCollider))]
    public class Brace : TwoClickBuildBlock
    {
        private float _length;
        public float Length => _length;

        private ProceduralCylinder _proceduralCylinder;
        private ProceduralCylinderMesh _gizmoMesh;
        private MeshCollider _meshCollider;

        protected override void OnEnterPlayMode()
        {
        }

        protected override void OnEnterBuildMode()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            if (_meshCollider == null)
                _meshCollider = GetComponent<MeshCollider>();

            if (_proceduralCylinder == null)
            {
                _proceduralCylinder = GetComponent<ProceduralCylinder>();
                UpdateProceduralModel();
            }
        }

        public void OnDrawGizmos()
        {
            if (block1 == null || block2 == null)
            {
                return;
            }

            if (_gizmoMesh == null)
                _gizmoMesh = new ProceduralCylinderMesh(0.2f, 0.2f, 1, 4, 1);

            // draw gizmo mesh
            Gizmos.color = Color.gray;
            (Vector3 center, Vector3 direction) = CalculatePositionAndDirectionVectors();
            Gizmos.DrawMesh(_gizmoMesh.UpdateMesh(0.2f, 0.2f, direction.magnitude), center,
                Quaternion.FromToRotation(Vector3.forward, direction));
        }

        private (Vector3, Vector3) CalculatePositionAndDirectionVectors()
        {
            Vector3 pos1 = block1.position;
            Vector3 pos2 = block2.position;

            Vector3 position = (pos1 + pos2) / 2;
            Vector3 direction = pos1 - pos2;

            return (position, direction);
        }

        private void OnValidate()
        {
            if (block1 != null && block2 != null)
            {
                (Vector3 center, Vector3 direction) = CalculatePositionAndDirectionVectors();
                transform.SetPositionAndRotation(center, Quaternion.FromToRotation(Vector3.forward, direction));
            }
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
            _length = direction.magnitude;
            _proceduralCylinder.SetHeight(_length);

            _meshCollider.sharedMesh = _proceduralCylinder.Mesh;
        }
    }
}