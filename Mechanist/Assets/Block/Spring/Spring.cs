using System;
using Core;
using MeshUtils;
using UnityEngine;
using UnityEngine.Assertions;

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
        private SpringJoint _joint;

        private Vector3 _prevPlug1Pos;
        private Vector3 _prevPlug2Pos;

        public override BlockType GetBlockType() => BlockType.Spring;

        protected override void Initialize()
        {
            // must be in Initialize()
            var go1 = new GameObject("plug1");
            _plug1 = go1.AddComponent<Rigidbody>();
            _plug1.useGravity = false;
            var go2 = new GameObject("plug2");
            _plug2 = go2.AddComponent<Rigidbody>();
            _plug2.useGravity = false;
        }

        protected override void LateInitialize()
        {
            base.LateInitialize();

            _plug1.position = block1.position;
            _plug2.position = block2.position;
            _prevPlug1Pos = _plug1.position;
            _prevPlug2Pos = _plug2.position;
        }

        protected override void Update()
        {
            base.Update();
            if (_gameMode == GameMode.PlayMode)
            {
                var p1 = _plug1.position;
                var p2 = _plug2.position;

                if (p1 != _prevPlug1Pos || p2 != _prevPlug2Pos)
                {
                    UpdateProceduralModel();
                    _prevPlug1Pos = p1;
                    _prevPlug2Pos = p2;
                }
            }
        }

        protected override void OnEnterPlayMode()
        {
            base.OnEnterPlayMode();

            _plug1.isKinematic = false;
            _plug2.isKinematic = false;
            _joint = _plug1.gameObject.AddComponent<SpringJoint>();
            _joint.connectedBody = _plug2;
            _joint.anchor = _plug1.transform.InverseTransformPoint(_plug2.position);
        }

        protected override void OnEnterBuildMode()
        {
            base.OnEnterBuildMode();

            Destroy(_joint);
            _plug1.isKinematic = true;
            _plug2.isKinematic = true;
        }

        public override Rigidbody GetPlugForAttachedBlock(SingleClickBuildBlock scbb)
        {
            Assert.IsNotNull(block1);
            Assert.IsNotNull(block2);
            Assert.IsNotNull(_plug1);
            Assert.IsNotNull(_plug2);

            if (scbb == block1.GetComponent<SingleClickBuildBlock>()) return _plug1;
            if (scbb == block2.GetComponent<SingleClickBuildBlock>()) return _plug2;

            throw new Exception("Unreachable");
        }

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