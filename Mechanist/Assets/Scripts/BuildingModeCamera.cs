using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class BuildingModeCamera : MonoBehaviour
{
    public Transform cameraPivot;
    public InputActionReference mouseLookInputActionRef;
    public InputActionReference moveCameraPivotInputActionRef;
    public float cameraRotateSpeed = 0.2f;
    public float distance = 10;
    public float moveSpeed = 0.2f;

    private bool _rotating = false;
    private Transform _transform;
    private Vector3 _pivotMoveDelta = Vector2.zero;

    public void Start()
    {
        _transform = GetComponent<Transform>();
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
            Vector3 delta = mouseLookInputActionRef.action.ReadValue<Vector2>();
            Vector3 axis = _transform.TransformDirection(Vector3.Cross(Vector3.forward, delta));

            _transform.RotateAround(cameraPivot.position, axis, delta.magnitude * cameraRotateSpeed);
        }

        // keep constant distance with the target
        var targetPos = cameraPivot.position;
        _transform.position = targetPos + (_transform.position - targetPos).normalized * distance;

        // always look at the target
        _transform.LookAt(cameraPivot);
    }

    public void OnRotate(InputAction.CallbackContext callbackContext)
    {
        _rotating = callbackContext.ReadValue<float>() > 0.001;
    }

    public void OnPivotMove(InputAction.CallbackContext callbackContext)
    {
        _pivotMoveDelta = callbackContext.ReadValue<Vector3>();
    }
}