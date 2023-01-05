using UnityEngine;
using MeshUtils;

namespace TransformHandle
{
    public class ScaleGlobal : HandleBase
    {
        protected Vector3 _axis;
        protected Vector3 _startScale;

        public ScaleGlobal Initialize(RuntimeTransformHandle p_parentTransformHandle, Vector3 p_axis, Color p_color)
        {
            _parentTransformHandle = p_parentTransformHandle;
            _axis = p_axis;
            _defaultColor = p_color;

            InitializeMaterial();

            transform.SetParent(p_parentTransformHandle.transform, false);

            GameObject o = new GameObject();
            o.layer = p_parentTransformHandle.gameObject.layer;
            o.transform.SetParent(transform, false);
            MeshRenderer mr = o.AddComponent<MeshRenderer>();
            mr.material = _material;
            MeshFilter mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshFactory.CreateBox(.35f, .35f, .35f);
            MeshCollider mc = o.AddComponent<MeshCollider>();

            return this;
        }

        public override void Interact(Vector3 p_previousPosition)
        {
            Vector3 mouseVector = (Input.mousePosition - p_previousPosition);
            float d = (mouseVector.x + mouseVector.y) * Time.deltaTime * 2;
            delta += d;
            _parentTransformHandle.TargetTransform.localScale = _startScale + Vector3.Scale(_startScale, _axis) * delta;

            base.Interact(p_previousPosition);
        }

        public override void StartInteraction(Vector3 p_hitPoint)
        {
            base.StartInteraction(p_hitPoint);
            _startScale = _parentTransformHandle.TargetTransform.localScale;
        }
    }
}