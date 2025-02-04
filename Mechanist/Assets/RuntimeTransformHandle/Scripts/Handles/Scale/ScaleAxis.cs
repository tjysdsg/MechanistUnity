using UnityEngine;
using MeshUtils;

namespace TransformHandle
{
    public class ScaleAxis : HandleBase
    {
        private const float SIZE = 2;

        private Vector3 _axis;
        private Vector3 _startScale;

        private float _interactionDistance;
        private Ray _raxisRay;

        public ScaleAxis Initialize(RuntimeTransformHandle p_parentTransformHandle, Vector3 p_axis, Color p_color)
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
            mf.mesh = MeshFactory.CreateCone(p_axis.magnitude * SIZE, .02f, .02f, 8, 1);
            MeshCollider mc = o.AddComponent<MeshCollider>();
            mc.sharedMesh = MeshFactory.CreateCone(p_axis.magnitude * SIZE, .1f, .02f, 8, 1);
            o.transform.localRotation = Quaternion.FromToRotation(Vector3.up, p_axis);

            o = new GameObject();
            o.layer = p_parentTransformHandle.gameObject.layer;
            o.transform.SetParent(transform, false);
            mr = o.AddComponent<MeshRenderer>();
            mr.material = _material;
            mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshFactory.CreateBox(.25f, .25f, .25f);
            mc = o.AddComponent<MeshCollider>();
            o.transform.localRotation = Quaternion.FromToRotation(Vector3.up, p_axis);
            o.transform.localPosition = p_axis * SIZE;

            return this;
        }

        protected void Update()
        {
            transform.GetChild(0).localScale = new Vector3(1, 1 + delta, 1);
            transform.GetChild(1).localPosition = _axis * (SIZE * (1 + delta));
        }

        public override void Interact(Vector3 p_previousPosition)
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            float closestT = HandleMathUtils.ClosestPointOnRay(_raxisRay, cameraRay);
            Vector3 hitPoint = _raxisRay.GetPoint(closestT);

            float distance = Vector3.Distance(_parentTransformHandle.TargetTransform.position, hitPoint);
            float axisScaleDelta = distance / _interactionDistance - 1f;

            Vector3 snapping = _parentTransformHandle.scaleSnap;
            float snap = Mathf.Abs(Vector3.Dot(snapping, _axis));
            if (snap != 0)
            {
                if (_parentTransformHandle.snappingType == HandleSnappingType.RELATIVE)
                {
                    axisScaleDelta = Mathf.Round(axisScaleDelta / snap) * snap;
                }
                else
                {
                    float axisStartScale = Mathf.Abs(Vector3.Dot(_startScale, _axis));
                    axisScaleDelta = Mathf.Round((axisScaleDelta + axisStartScale) / snap) * snap - axisStartScale;
                }
            }

            delta = axisScaleDelta;
            Vector3 scale = Vector3.Scale(_startScale, _axis * axisScaleDelta + Vector3.one);

            _parentTransformHandle.TargetTransform.localScale = scale;

            base.Interact(p_previousPosition);
        }

        public override void StartInteraction(Vector3 p_hitPoint)
        {
            base.StartInteraction(p_hitPoint);
            _startScale = _parentTransformHandle.TargetTransform.localScale;

            Vector3 raxis = _parentTransformHandle.space == HandleSpace.LOCAL
                ? _parentTransformHandle.TargetTransform.rotation * _axis
                : _axis;

            _raxisRay = new Ray(_parentTransformHandle.TargetTransform.position, raxis);

            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            float closestT = HandleMathUtils.ClosestPointOnRay(_raxisRay, cameraRay);
            Vector3 hitPoint = _raxisRay.GetPoint(closestT);

            _interactionDistance = Vector3.Distance(_parentTransformHandle.TargetTransform.position, hitPoint);
        }
    }
}