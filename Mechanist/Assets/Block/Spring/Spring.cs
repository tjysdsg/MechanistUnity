using MeshUtils;
using UnityEngine;

namespace Block
{
    public class Spring : Beam
    {
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _cylinderRadius = 0.05f;
        [SerializeField] private int _nCircles = 5;
        [SerializeField] private int _nRadialSegments = 16;
        [SerializeField] private int _nSegments = 100;

        private ProceduralSpringMesh _proceduralMesh = null;
        private Rigidbody _plug1;
        private Rigidbody _plug2;

        // public override BlockType GetBlockType() => BlockType.Spring;

        // public override Rigidbody GetPlugForAttachedBlock(SingleClickBuildBlock scbb)
        // {
        //     Assert.IsNotNull(block1);
        //     Assert.IsNotNull(block2);
        //     Assert.IsNotNull(_plug1);
        //     Assert.IsNotNull(_plug2);

        //     if (scbb == block1.GetComponent<SingleClickBuildBlock>()) return _plug1;
        //     if (scbb == block2.GetComponent<SingleClickBuildBlock>()) return _plug2;

        //     throw new Exception("Unreachable");
        // }

        public override void UpdateProceduralModel()
        {
            if (_proceduralMesh == null)
                _proceduralMesh = new ProceduralSpringMesh(
                    _radius, _cylinderRadius, length, _nCircles, _nRadialSegments, _nSegments
                );

            (Vector3 center, Vector3 direction) = CalculatePositionAndDirectionVectors();

            // update transform
            transform.SetPositionAndRotation(center, Quaternion.FromToRotation(Vector3.forward, direction));

            // update mesh
            length = direction.magnitude;
            GetComponent<MeshFilter>().sharedMesh = _proceduralMesh.UpdateMesh(length);
            GetComponent<MeshCollider>().sharedMesh = _proceduralMesh.Mesh;
        }
    }
}