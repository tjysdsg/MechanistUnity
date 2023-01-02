using System;
using GameState;
using UnityEngine;
using UnityEngine.Serialization;

namespace BuildMode
{
    public class BuildModeCamera : MonoBehaviour
    {
        public Transform cameraPivot;
        public InputManager inputManager;
        public float cameraRotateSpeed = 0.2f;

        public float distance = 10;
        public float moveSpeed = 0.1f;
        public float slideSpeed = 0.1f;

        [SerializeField] private CurrentCameraSO currentCamera;

        private bool _rotating = false;
        private bool _dragging = false;
        private Transform _transform;
        private Vector3 _pivotMoveDelta = Vector2.zero;

        private Camera _camera;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _camera = GetComponent<Camera>();
            currentCamera.camera = _camera;
        }

        private void OnEnable()
        {
            inputManager.BuildingModeRotateCameraEvent += OnRotateCamera;
            inputManager.BuildingModeDragCameraEvent += OnDragCamera;
            inputManager.BuildingModeZoomEvent += OnZoom;
            inputManager.BuildingModeMoveCameraPivotEvent += OnCameraPivotMoveCamera;
        }

        private void OnDisable()
        {
            inputManager.BuildingModeRotateCameraEvent -= OnRotateCamera;
            inputManager.BuildingModeDragCameraEvent -= OnDragCamera;
            inputManager.BuildingModeZoomEvent -= OnZoom;
            inputManager.BuildingModeMoveCameraPivotEvent -= OnCameraPivotMoveCamera;
        }

        public void Update()
        {
            // move camera pivot
            Vector3 translation = _transform.TransformDirection(_pivotMoveDelta * moveSpeed);
            cameraPivot.Translate(translation, Space.World);
            _transform.Translate(translation, Space.World);

            // rotate camera based on input
            Vector2 lookDelta = inputManager.GetBuildModePointerDeltaInput();
            if (_rotating)
            {
                Vector3 axis = _transform.TransformDirection(Vector3.Cross(Vector3.forward, lookDelta));

                _transform.RotateAround(cameraPivot.position, axis, lookDelta.magnitude * cameraRotateSpeed);
            }
            else if (_dragging)
            {
                Vector3 delta = -lookDelta;
                _transform.Translate(delta * slideSpeed, Space.Self);
                cameraPivot.Translate(_transform.TransformDirection(delta) * slideSpeed, Space.World);
            }

            // keep constant distance with the target
            var targetPos = cameraPivot.position;
            _transform.position = targetPos + (_transform.position - targetPos).normalized * distance;

            // always look at the target
            _transform.LookAt(cameraPivot);
        }

        public void OnZoom(float zoom)
        {
            zoom = Mathf.Clamp(zoom, -10f, 10f);
            _pivotMoveDelta = Vector3.forward * zoom;
        }

        public void OnCameraPivotMoveCamera(Vector3 v)
        {
            _pivotMoveDelta = v;
        }

        public void OnRotateCamera(float val)
        {
            _rotating = val > 0.001;
        }

        public void OnDragCamera(float val)
        {
            _dragging = !_rotating && val > 0.001;
        }

        /// <summary>
        /// Move pivot to <paramref name="pos"/>
        /// Raised by <see cref="BuildModeManager"/>
        /// </summary>
        public void OnMoveTo(Vector3 pos)
        {
            Vector3 delta = pos - cameraPivot.position;
            _transform.position += delta;
            cameraPivot.position = pos;
        }
    }
}