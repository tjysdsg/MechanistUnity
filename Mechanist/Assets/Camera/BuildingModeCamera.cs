using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingModeCamera : MonoBehaviour
{
    public Transform cameraPivot;
    public InputManager inputManager;
    public float cameraRotateSpeed = 0.2f;

    public float distance = 10;
    public float minDistance = 2;
    public float maxDistance = 30;

    public float moveSpeed = 0.1f;
    public float slideSpeed = 0.1f;
    public float zoomSpeed = 0.2f;

    private bool _rotating = false;
    private bool _dragging = false;
    private Vector2 _lookDelta;
    private Transform _transform;
    private Vector3 _pivotMoveDelta = Vector2.zero;

    public void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        inputManager.BuildingModeRotateCameraEvent += OnRotateCamera;
        inputManager.BuildingModeDragCameraEvent += OnDragCamera;
        inputManager.BuildingModeZoomEvent += OnZoom;
        inputManager.BuildingModeMoveCameraPivotEvent += OnCameraPivotMoveCamera;
        inputManager.BuildingModeLookEvent += OnLook;
    }

    private void OnDisable()
    {
        inputManager.BuildingModeRotateCameraEvent -= OnRotateCamera;
        inputManager.BuildingModeDragCameraEvent -= OnDragCamera;
        inputManager.BuildingModeZoomEvent -= OnZoom;
        inputManager.BuildingModeMoveCameraPivotEvent -= OnCameraPivotMoveCamera;
        inputManager.BuildingModeLookEvent -= OnLook;
    }

    public void Update()
    {
        // move camera pivot
        Vector3 translation = _transform.TransformDirection(_pivotMoveDelta * moveSpeed);
        cameraPivot.Translate(translation, Space.World);
        _transform.Translate(translation, Space.World);

        // rotate camera based on input
        if (_rotating)
        {
            Vector3 axis = _transform.TransformDirection(Vector3.Cross(Vector3.forward, _lookDelta));

            _transform.RotateAround(cameraPivot.position, axis, _lookDelta.magnitude * cameraRotateSpeed);
        }
        else if (_dragging)
        {
            Vector3 delta = -_lookDelta;
            _transform.Translate(delta * slideSpeed, Space.Self);
            cameraPivot.Translate(_transform.TransformDirection(delta) * slideSpeed, Space.World);
        }

        // keep constant distance with the target
        var targetPos = cameraPivot.position;
        _transform.position = targetPos + (_transform.position - targetPos).normalized * distance;

        // always look at the target
        _transform.LookAt(cameraPivot);
    }

    public void OnLook(Vector2 v)
    {
        _lookDelta = v;
    }

    public void OnZoom(float zoom)
    {
        zoom = Mathf.Clamp(zoom, -10f, 10f);
        distance -= zoom * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
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
}