using UnityEngine;
using MeshUtils;

namespace Block
{
    public class Brace : BaseBlock
    {
        [SerializeField] public Transform block1;
        [SerializeField] public Transform block2;
        [SerializeField] public GameObject cylinderModelPrefab;

        private float _length;
        public float Length => _length;

        private GameObject _cylinderModel;
        private ProceduralCylinder _proceduralCylinder;
        private ProceduralCylinderMesh _gizmoMesh;

        public override void Initialize()
        {
            base.Initialize();

            if (_cylinderModel == null)
            {
                _cylinderModel = Instantiate(cylinderModelPrefab, Vector3.zero, Quaternion.identity);
                _cylinderModel.transform.parent = transform;
                _proceduralCylinder = _cylinderModel.GetComponent<ProceduralCylinder>();
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
                Quaternion.FromToRotation(Vector3.up, direction));
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
            _cylinderModel.transform.localScale = Vector3.one;
            _cylinderModel.transform.SetPositionAndRotation(center, Quaternion.FromToRotation(Vector3.up, direction));

            // generate/update cylinder mesh
            _length = direction.magnitude;
            _proceduralCylinder.SetHeight(_length);
        }
    }
}